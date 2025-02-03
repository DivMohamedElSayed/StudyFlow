namespace StudyFlow.API.Entities;

public class JwtOptions
{
    public static string SectionName = "Jwt";

    [Required(AllowEmptyStrings = false, ErrorMessage = "AccessToken is required.")]
    public string AccessToken { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Issuer is required.")]
    public string Issuer { get; init; } = string.Empty;

    [Required(AllowEmptyStrings = false, ErrorMessage = "Audience is required.")]
    public string Audience { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Expiration time must be at least 1 minute.")]
    public int ExpireIn { get; init; }
}