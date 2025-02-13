namespace StudyFlow.API.Abstractions;

public class Result
{
    public Result(bool isSuccess, Error error,string message)
    {
        if (isSuccess && error != Error.None || !isSuccess && error == Error.None)
        {
            throw new InvalidOperationException(
                "Invalid Result state: A successful result cannot have an error, and a failure must have an error."
            );
        }
        IsSuccess = isSuccess;
        Error = error;
        Message = message!;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error Error { get; } = default!;
    public string Message { get; } = string.Empty;

    public static Result Success() => new(true, Error.None,"");

    public static Result Failure(Error error) => new(false, error,string.Empty);

    public static Result<TValue> Success<TValue>(TValue value, string message = "Operation successful") => new(value, true, Error.None,message);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error,string.Empty);
}

public class Result<TValue>(TValue? value, bool isSuccess, Error error, string message) : Result(isSuccess, error,message)
{
    private TValue? _value = value;

    public TValue Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("Cannot access value of a failed result.");
}