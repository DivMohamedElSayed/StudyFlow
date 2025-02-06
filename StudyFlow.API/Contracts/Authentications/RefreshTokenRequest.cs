namespace StudyFlow.API.Contracts.Authentications;

public record RefreshTokenRequest(
    string AccessToken,
    string RefreshToken
);
