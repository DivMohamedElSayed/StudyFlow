namespace StudyFlow.API.Contracts.Authentications;

public record AuthResponse(
    string Id,
    string FirstName,
    string LastName,
    string? Email,
    string? UserName,
    string AccessToken,
    int ExpireIn
);