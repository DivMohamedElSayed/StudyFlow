namespace StudyFlow.API.Contracts.Users;

public record UserThemeResponse(string ThemePreference)
{
    public static UserThemeResponse FromUser(ApplicationUser user) =>
    new(user.ThemePreference); 
}