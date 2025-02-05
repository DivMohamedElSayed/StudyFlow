namespace StudyFlow.API.Contracts.Authentications.Filters;

public class RejectExtraFieldsMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.ContentType?.Contains("application/json") == true)
        {
            try
            {
                // Enable buffering so we can read the body multiple times
                context.Request.EnableBuffering();

                using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
                var body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset body stream for next middleware

                // Parse JSON body
                using var jsonDoc = JsonDocument.Parse(body);

                // Get expected model type from the request
                var endpoint = context.GetEndpoint();
                var parameters = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor>()?.Parameters;

                if (parameters != null && parameters.Count > 0)
                {
                    var expectedType = parameters.FirstOrDefault()?.ParameterType;
                    if (expectedType != null)
                    {
                        var expectedProperties = expectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                            .Select(p => p.Name)
                            .ToHashSet(StringComparer.OrdinalIgnoreCase);

                        // Find extra fields in the JSON request
                        var extraFields = jsonDoc.RootElement.EnumerateObject()
                            .Where(p => !expectedProperties.Contains(p.Name))
                            .Select(p => p.Name)
                            .ToList();

                        if (extraFields.Any())
                        {
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            context.Response.ContentType = "application/json";
                            var responseMessage = JsonSerializer.Serialize(new
                            {
                                message = "Request contains unknown fields.",
                                extraFields
                            });
                            await context.Response.WriteAsync(responseMessage);
                            return;
                        }
                    }
                }
            }
            catch (JsonException)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync("{\"message\": \"Invalid JSON format.\"}");
                return;
            }
        }

        await _next(context);
    }
}