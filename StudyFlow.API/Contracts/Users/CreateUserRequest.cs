namespace StudyFlow.API.Contracts.Users;

public record CreateUserRequest(
    string FirstName,
    string LastName,
    string PhoneNumber
);
