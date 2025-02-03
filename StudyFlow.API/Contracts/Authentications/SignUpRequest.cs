namespace StudyFlow.API.Contracts.Authentications;

public record SignUpRequest(
    string FirstName,
    string LastName,
    string Email,
    string UserName,
    string Password
);