namespace StudyFlow.API.Abstractions;

public class ResultResponse<T>
{
    public T? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public DateTime TimeStamp { get; set; }
}
