namespace StudyFlow.API.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }
    public string ThemePreference { get; set; } = ThemeConstants.Default;
}