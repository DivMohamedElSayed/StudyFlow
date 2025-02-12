namespace StudyFlow.API.Contracts.Authentications;

public record AuthResponseSignUp(
    string Id,
    string AccessToken,
    int ExpireIn,
    string RefreshToken,
    DateTime RefreshTokenExpirations
);
