namespace StudyFlow.API.Contracts.Users;

public record UserProfileResponse(
    string FirstName,
    string LastName,
    string UserName
);