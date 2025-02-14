namespace StudyFlow.API.Entities;

public sealed class Teacher
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string Subject { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser ApplicationUser { get; set; } = default!;
}
