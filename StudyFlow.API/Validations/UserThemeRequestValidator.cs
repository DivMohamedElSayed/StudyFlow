namespace StudyFlow.API.Validations;

public class UserThemeRequestValidator : AbstractValidator<UserThemeRequest>
{
    public UserThemeRequestValidator()
    {
        RuleFor(t => t.ThemePreference)
            .NotEmpty().WithMessage("Theme preference cannot be empty.")
            .Must(value => ThemeConstants.ValidThemes.Contains(value.ToLower()))
            .WithMessage($"Invalid theme preference. Allowed values: {string.Join(", ", ThemeConstants.ValidThemes)}");
    }
}