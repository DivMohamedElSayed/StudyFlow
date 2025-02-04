namespace StudyFlow.API.Contracts.Authentications;

public record SignUpRequest(
    string Email,
    string UserName,
    string Password
);