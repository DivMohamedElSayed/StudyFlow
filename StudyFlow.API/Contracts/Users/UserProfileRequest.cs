namespace StudyFlow.API.Contracts.Users;

public record UserProfileRequest(
    string FirstName,
    string LastName
);