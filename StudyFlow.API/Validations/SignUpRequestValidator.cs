namespace StudyFlow.API.Validations;

public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
{
    public SignUpRequestValidator()
    {
        RuleFor(f => f.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required.")
            .Length(3, 100)
            .WithMessage("First Name must be between 3 and 100 characters.");

        RuleFor(l => l.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required.")
            .Length(3, 100)
            .WithMessage("Last Name must be between 3 and 100 characters.");

        RuleFor(e => e.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Invalid email format.");

        RuleFor(p => p.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Matches(RegexPattern.Password)
            .WithMessage("Password must be at least 8 characters long, include uppercase, lowercase, a number, and a special character.");

        RuleFor(u => u.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .Matches(RegexPattern.UserName)
            .WithMessage("Username must be 3-20 characters long and can only contain letters, numbers, and underscores.");
    }
}