namespace StudyFlow.API.Validations;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(u => u.UserName)
           .NotEmpty()
           .WithMessage("Username is required.")
           .Matches(RegexPattern.UserName)
           .WithMessage("Username must be 3-20 characters long and can only contain letters, numbers, and underscores.");
        RuleFor(p => p.Password)
           .NotEmpty()
           .WithMessage("Password is required.")
           .Matches(RegexPattern.Password)
           .WithMessage("Password must be at least 8 characters long, include uppercase, lowercase, a number, and a special character.");
    }
}