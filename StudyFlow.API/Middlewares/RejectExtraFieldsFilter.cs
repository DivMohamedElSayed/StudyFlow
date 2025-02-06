namespace StudyFlow.API.Middlewares;

public class RejectExtraFieldsMiddleware
{
    private readonly RequestDelegate _next; // The next middleware in the pipeline
    private readonly ILogger<RejectExtraFieldsMiddleware> _logger; // Logger for logging warnings/errors
    private readonly JsonSerializerOptions _jsonOptions; // JSON serialization options (case-insensitive)

    public RejectExtraFieldsMiddleware(
        RequestDelegate next,
        ILogger<RejectExtraFieldsMiddleware> logger)
    {
        _next = next; // Initialize the next middleware
        _logger = logger; // Initialize the logger
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Configure JSON options to be case-insensitive
        };
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request content type is JSON
        if (!IsJsonContentType(context.Request))
        {
            await _next(context); // If not JSON, skip validation and pass to the next middleware
            return;
        }

        try
        {
            // Validate the request payload for extra fields
            var (isValid, extraFields) = await ValidateRequestPayload(context);
            if (!isValid)
            {
                // If extra fields are found, log them and reject the request
                LogUnexpectedFields(extraFields, context);
                await HandleInvalidRequest(context, extraFields);
                return;
            }

            // If no extra fields, pass the request to the next middleware
            await _next(context);
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during processing
            await HandleException(context, ex);
        }
    }

    // Helper method to check if the request content type is JSON
    private bool IsJsonContentType(HttpRequest request) =>
        request.ContentType?.Contains("application/json") == true;

    // Validate the request payload against the expected model type
    private async Task<(bool isValid, List<string> extraFields)> ValidateRequestPayload(HttpContext context)
    {
        context.Request.EnableBuffering(); // Enable buffering to read the request body multiple times
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        var body = await reader.ReadToEndAsync(); // Read the request body as a string
        context.Request.Body.Position = 0; // Reset the stream position for further processing

        // Get the endpoint and its expected parameters
        var endpoint = context.GetEndpoint();
        var parameters = endpoint?.Metadata.GetMetadata<ActionDescriptor>()?.Parameters;

        // If no parameters are expected, the request is valid
        if (parameters == null || parameters.Count == 0)
            return (true, new List<string>());

        // Get the expected model type from the first parameter
        var expectedType = parameters.FirstOrDefault()?.ParameterType;
        return expectedType == null
            ? (true, new List<string>()) // If no expected type, the request is valid
            : ValidateJsonAgainstModel(body, expectedType); // Validate the JSON against the expected type
    }

    // Validate the JSON body against the expected model type
    private (bool isValid, List<string> extraFields) ValidateJsonAgainstModel(string jsonBody, Type expectedType)
    {
        // Get all public properties of the expected model type (case-insensitive)
        var expectedProperties = expectedType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name.ToLower())
            .ToHashSet();

        // Parse the JSON body and find extra fields not in the expected model
        using var jsonDoc = JsonDocument.Parse(jsonBody);
        var extraFields = jsonDoc.RootElement.EnumerateObject()
            .Where(p => !expectedProperties.Contains(p.Name.ToLower())) // Check for unexpected fields
            .Select(p => p.Name)
            .ToList();

        // Return whether the request is valid and the list of extra fields
        return (extraFields.Count == 0, extraFields);
    }

    // Log unexpected fields detected in the request
    private void LogUnexpectedFields(List<string> extraFields, HttpContext context)
    {
        _logger.LogWarning(
            "Unexpected fields detected in request from {RemoteIpAddress}: {ExtraFields}",
            context.Connection.RemoteIpAddress, // Log the client's IP address
            string.Join(", ", extraFields) // Log the list of extra fields
        );
    }

    // Handle invalid requests by returning a 400 Bad Request response
    private async Task HandleInvalidRequest(HttpContext context, List<string> extraFields)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest; // Set status code to 400
        context.Response.ContentType = "application/json"; // Set response content type to JSON

        // Create a response message with details about the extra fields
        var responseMessage = JsonSerializer.Serialize(new
        {
            message = "Request contains unknown fields.",
            extraFields
        }, _jsonOptions);

        // Write the response message to the client
        await context.Response.WriteAsync(responseMessage);
    }

    // Handle exceptions by logging the error and returning a 500 Internal Server Error response
    private async Task HandleException(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Error processing request"); // Log the exception
        context.Response.StatusCode = StatusCodes.Status500InternalServerError; // Set status code to 500
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            message = "An error occurred processing the request." // Return a generic error message
        }));
    }
}

// Extension method to register the middleware in the application pipeline
public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRejectExtraFields(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RejectExtraFieldsMiddleware>(); // Register the middleware
    }
}