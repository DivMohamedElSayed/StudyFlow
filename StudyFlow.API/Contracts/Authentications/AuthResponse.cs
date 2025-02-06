namespace StudyFlow.API.Contracts.Authentications;

public record AuthResponse(
    string Id,
    string? Email,
    string? UserName,
    string AccessToken,
    int ExpireIn,
    string RefreshToken,
    DateTime RefreshTokenExpirations
);