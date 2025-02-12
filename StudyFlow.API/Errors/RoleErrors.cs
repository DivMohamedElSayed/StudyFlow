namespace StudyFlow.API.Errors;

public static class RoleErrors
{
    public static readonly Error RoleNotFound =
        new("Role.NotFound", "The specified role could not be found.", StatusCodes.Status404NotFound);

    public static readonly Error InvalidRole =
        new("Role.Invalid", "The specified role is invalid.", StatusCodes.Status400BadRequest);

}
