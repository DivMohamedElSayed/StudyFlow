namespace StudyFlow.API.Validations;

public class SignInRequestValidator : AbstractValidator<SignInRequest>
{
    public SignInRequestValidator()
    {
        RuleFor(u => u.UserName)
            .NotEmpty()
            .WithMessage("Username is required.")
            .Length(3, 20)
            .WithMessage("Username must be 3-20 characters long.")
            .Matches(RegexPattern.UserName)
            .WithMessage("Username can only contain lowercase letters, numbers, and underscores.")
            .Must(username => !username.Contains("#"))
            .WithMessage("Username cannot contain the '#' character.");

        RuleFor(p => p.Password)
           .NotEmpty()
           .WithMessage("Password is required.")
           .Matches(RegexPattern.Password)
           .WithMessage("Password must be at least 8 characters long, include uppercase, lowercase, a number, and a special character.");
    }
}