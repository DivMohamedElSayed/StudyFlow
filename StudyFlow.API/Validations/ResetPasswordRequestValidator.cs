namespace StudyFlow.API.Validations;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(e => e.Email)
        .NotEmpty()
        .WithMessage("Email is required.")
        .EmailAddress()
        .WithMessage("Invalid email format.");

        RuleFor(n => n.NewPassword)
            .NotEmpty()
            .WithMessage("New password is required.")
            .Matches(RegexPattern.Password)
            .WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");

        RuleFor(c => c.code)
            .NotEmpty()
            .WithMessage("Access Token is required.");
    }
}
