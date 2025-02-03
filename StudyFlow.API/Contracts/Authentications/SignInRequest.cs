namespace StudyFlow.API.Contracts.Authentications;

public record SignInRequest(
    string Email,
    string UserName,
    string Password
);