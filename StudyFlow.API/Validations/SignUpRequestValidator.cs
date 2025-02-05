namespace StudyFlow.API.Validations
{
    public class SignUpRequestValidator : AbstractValidator<SignUpRequest>
    {
        public SignUpRequestValidator()
        {
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
            .Length(3, 20)
            .WithMessage("Username must be 3-20 characters long.")
            .Matches(RegexPattern.UserName)
            .WithMessage("Username can only contain lowercase letters, numbers, and underscores.")
            .Must(username => !username.Contains("#"))
            .WithMessage("Username cannot contain the '#' character.");

        }
    }
}
