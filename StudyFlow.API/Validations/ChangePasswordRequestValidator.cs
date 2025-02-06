namespace StudyFlow.API.Validations;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(c => c.CurrentPassword)
            .NotEmpty()
            .WithMessage("Current password is required.");
        RuleFor(n => n.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .Matches(RegexPattern.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
    }
}
