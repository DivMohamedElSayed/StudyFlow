namespace StudyFlow.API.Contracts.Authentications;

public record SignInRequest(
    string UserName,
    string Password
);