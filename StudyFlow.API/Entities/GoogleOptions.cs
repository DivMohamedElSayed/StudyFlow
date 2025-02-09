namespace StudyFlow.API.Entities;

public class GoogleOptions
{
    public const string SectionName = "Authentication:Google";
    [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required.")]
    public string ClientId { get; init; } = string.Empty;
    [Required(AllowEmptyStrings = false, ErrorMessage = "ClientSecret is required.")]
    public string ClientSecret { get; init; } = string.Empty;
}
