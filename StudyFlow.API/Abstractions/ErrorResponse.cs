namespace StudyFlow.API.Abstractions;

public class ErrorResponse
{
    public object? Data { get; set; }
    public string Message { get; set; } = string.Empty;
    public int Status { get; set; }
    public Dictionary<string, List<string>> Errors { get; set; } = [];
}
