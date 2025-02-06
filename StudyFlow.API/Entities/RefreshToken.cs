namespace StudyFlow.API.Entities;

[Owned]
public class RefreshToken
{
    public string RefreshTokenCode { get; set; } = string.Empty;
    public DateTime ExpireOn { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedOn { get; set; }
    public bool IsExpired => DateTime.UtcNow >= ExpireOn;
    public bool IsActive => RevokedOn is null && !IsExpired;
}
