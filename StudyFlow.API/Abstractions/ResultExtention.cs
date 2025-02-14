namespace StudyFlow.API.Abstractions;

public static class ResultExtension
{
    public static ObjectResult ToProblem(this Result result)
    {
        // Check result
        if (result.IsSuccess)
            throw new InvalidOperationException();
        // Problem to Show the type of Error
        var problem = Results.Problem(statusCode: result.Error.Status);
        // Convert Problem Details
        var problemDetails = problem.GetType().GetProperty(nameof(ProblemDetails))!.GetValue(problem) as ProblemDetails;// Casting To Problem Details.
        problemDetails!.Extensions = new Dictionary<string, object?>
        {
            {
                "errors" , new[]
                {
                    result.Error.Code,
                    result.Error.Description
                }
            }
        };
        return new ObjectResult(problemDetails);
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