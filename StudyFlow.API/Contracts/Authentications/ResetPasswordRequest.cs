namespace StudyFlow.API.Contracts.Authentications;

public record ResetPasswordRequest(
    string code,
    string Email,
    string NewPassword
);
