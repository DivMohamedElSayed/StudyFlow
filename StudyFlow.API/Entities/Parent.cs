namespace StudyFlow.API.Entities;

public sealed class Parent
{
    public Guid Id { get; set; } = Guid.CreateVersion7();
    public string UserId { get; set; } = default!;

    public ICollection<ApplicationUser> ApplicationUsers { get; set; } = [];
}
