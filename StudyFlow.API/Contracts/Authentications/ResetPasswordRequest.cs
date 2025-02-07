namespace StudyFlow.API.Contracts.Authentications;

public record ResetPasswordRequest(
    string AccessToken,
    string Email,
    string NewPassword
);
