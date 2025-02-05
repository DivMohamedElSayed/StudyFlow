namespace StudyFlow.API.Validations;

public class UserProfileRequestValidator : AbstractValidator<UserProfileRequest>
{
    public UserProfileRequestValidator()
    {
        RuleFor(f => f.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .Length(3, 100)
            .WithMessage("First name must be between 3 and 100 characters.");

        RuleFor(l => l.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .Length(3, 100)
            .WithMessage("Last name must be between 3 and 100 characters.");
    }
}