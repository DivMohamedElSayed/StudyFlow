using System.Security.Cryptography.Xml;

namespace StudyFlow.API.Errors;

public static class UserErrors
{
    public static readonly Error DuplicatedEmail =
        new("user.duplicate_email", "This email is already registered. Please use a different email.", StatusCodes.Status409Conflict);

    public static readonly Error DuplicatedUserName =
        new("user.duplicate_username", "This username is already taken. Please choose a different username.", StatusCodes.Status409Conflict);

    public static readonly Error InvalidCredentials =
    new("user.invalid_credentials", "Invalid username or password. Please try again.", StatusCodes.Status401Unauthorized);

    public static readonly Error UserIsDisabled =
    new("user.user_disabled", "This account has been disabled. Please contact support.", StatusCodes.Status403Forbidden);

    public static readonly Error EmailNotConfirmed =
    new("user.email_not_confirmed", "Your email address is not confirmed. Please check your inbox and confirm your email.", StatusCodes.Status401Unauthorized);

    public static readonly Error LockedOut =
    new("user.account_locked", "Your account is temporarily locked due to multiple failed login attempts. Please try again later or contact support.", StatusCodes.Status423Locked);

    public static readonly Error UserNotFound =
    new("user.not_found", "The user was not found. Please check the provided information and try again.", StatusCodes.Status404NotFound);

    public static readonly Error ThemeNotFound =
        new("user.ThemeNotFound", "Invalid theme preference. Allowed values: dark, light, default.", StatusCodes.Status404NotFound);
    public static readonly Error InvalidJwtToken =
    new("user.InvalidJwtToken", "The provided JWT token is invalid or expired.", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidRefreshToken =
    new("user.InvalidRefreshToken", "The provided refresh token is invalid or expired.", StatusCodes.Status400BadRequest);
    public static readonly Error ExpiredRefreshToken =
    new("Auth.ExpiredRefreshToken", "The provided refresh token has expired.", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidCode =
    new("User.InvalidCode", "The provided code is invalid or has expired. Please verify the code and try again.", StatusCodes.Status401Unauthorized);
    public static readonly Error UserCreationFailed =
        new("user.UserCreationFailed", "Failed to create the user. Please check the provided details and try again.", StatusCodes.Status400BadRequest);
    public static readonly Error GoogleAuthFailed =
        new("user.GoogleAuthFailed", "Google authentication failed. Please check your credentials and try again.", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidUserData =
        new("user.InvalidUserData", "First name and last name are required", StatusCodes.Status400BadRequest);
    public static readonly Error DuplicatedConfirmation =
    new("user.ConfirmationDuplicated", "The confirmation request has already been processed.", StatusCodes.Status409Conflict);

}