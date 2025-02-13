namespace StudyFlow.API.Abstractions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert a successful result to a problem response.");

        var problem = Results.Problem(statusCode: result.Error.StatusCode);
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails
            ?? throw new InvalidOperationException("Failed to extract ProblemDetails.");

        // Ensure existing extensions and add error details
        problemDetails.Extensions ??= new Dictionary<string, object?>();
        problemDetails.Extensions["errors"] = new[]
        {
            result.Error.Code,
            result.Error.Description
        };

        return new ObjectResult(problemDetails)
        {
            StatusCode = result.Error.StatusCode
        };
    }
    public static ObjectResult ToResponse<T>(this Result<T> result)
    {
        var statusCode = result.IsSuccess ? StatusCodes.Status200OK : result.Error.StatusCode ?? StatusCodes.Status400BadRequest;

        var response = new ResultResponse<T>
        {
            Data = result.IsSuccess ? result.Value : default,
            Message = result.Message,
            StatusCode = statusCode,
            TimeStamp = DateTime.UtcNow
        };

        return new ObjectResult(response) { StatusCode = statusCode };

    }

}