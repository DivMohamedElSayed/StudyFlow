namespace StudyFlow.API.Entities;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public bool IsDisabled { get; set; }
    public string ThemePreference { get; set; } = ThemeConstants.Default;
    // External authentication properties
    public string Provider { get; set; } = AuthProvider.Local;
    public string? ExternalProviderId { get; set; }
    // Helper property to check if user is externally authenticated
    public bool IsExternalUser => Provider != AuthProvider.Local;


    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public Student Student { get; set; } = default!;
    public Teacher Teacher { get; set; } = default!;
    public Parent Parent { get; set; } = default!;
    public Guid? ParentId { get; set; }
}