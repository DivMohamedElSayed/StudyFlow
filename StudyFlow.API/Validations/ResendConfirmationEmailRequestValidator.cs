namespace StudyFlow.API.Validations;

public class ResendConfirmationEmailRequestValidator : AbstractValidator<ResendConfirmationEmailRequest>
{
    public ResendConfirmationEmailRequestValidator()
    {
        RuleFor(u => u.UserName)
           .NotEmpty()
           .WithMessage("Username is required.")
           .Length(3, 20)
           .WithMessage("Username must be 3-20 characters long.")
           .Matches(RegexPattern.UserName)
           .WithMessage("Username can only contain lowercase letters, numbers, and underscores.")
           .Must(username => !username.Contains('#'))
           .WithMessage("Username cannot contain the '#' character.");
    }
}
