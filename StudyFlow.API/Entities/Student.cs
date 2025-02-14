namespace StudyFlow.API.Entities;

public sealed class Student
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public string Grade { get; set; } = string.Empty;

    public ApplicationUser ApplicationUser { get; set; } = default!;
}
