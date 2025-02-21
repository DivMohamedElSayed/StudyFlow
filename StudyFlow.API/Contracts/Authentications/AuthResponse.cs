namespace StudyFlow.API.Contracts.Authentications;

public record AuthResponseSignIn(
    string Id,
    string FirstName,
    string LastName,
    string? Email,
    string? UserName,
    string AccessToken,
    int ExpireIn,
    string RefreshToken,
    DateTime RefreshTokenExpirations
);