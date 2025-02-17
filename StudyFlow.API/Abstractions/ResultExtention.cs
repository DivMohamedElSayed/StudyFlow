namespace StudyFlow.API.Abstractions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot convert a successful result to a problem.");
        if (result.Error is null)
            throw new InvalidOperationException("Error object cannot be null for a failed result.");

        // Create the error response
        var errorResponse = new ErrorResponse
        {
            Data = null,
            Message = "One or more errors occurred.",
            Status = result.Error.Status ?? StatusCodes.Status400BadRequest,
            Errors = new Dictionary<string, List<string>>
            {
                {
                    result.Error.Code,
                    new List<string> { result.Error.Description }
                }
            }
        };
        return new ObjectResult(errorResponse)
        {
            StatusCode = errorResponse.Status
        };
    }
    public static ObjectResult ToResponse<T>(this Result<T> result)
    {
        var statusCode = result.IsSuccess ? StatusCodes.Status200OK : result.Error.Status ?? StatusCodes.Status400BadRequest;

        var response = new ResultResponse<T>
        {
            Data = result.IsSuccess ? result.Value : default,
            Message = result.Message,
            Status = statusCode
        };

        return new ObjectResult(response) { StatusCode = statusCode };
    }
}