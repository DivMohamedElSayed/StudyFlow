namespace StudyFlow.API.Contracts.Authentications;

public record ConfirmEmailRequest(
    string UserId,
    string Code
);
