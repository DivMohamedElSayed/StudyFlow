namespace StudyFlow.API.Entities;

public sealed class UploudedFile
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string FileName { get; set; } = string.Empty;
    public string StoredFileName { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string FileExtention { get; set; } = string.Empty;
}
