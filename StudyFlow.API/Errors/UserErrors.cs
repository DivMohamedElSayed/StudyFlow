namespace StudyFlow.API.Errors;

public static class UserErrors
{
    public static readonly Error DuplicatedEmail =
        new("user.duplicate_email", "This email is already registered. Please use a different email.", StatusCodes.Status409Conflict);

    public static readonly Error DuplicatedUserName =
        new("user.duplicate_username", "This username is already taken. Please choose a different username.", StatusCodes.Status409Conflict);
}