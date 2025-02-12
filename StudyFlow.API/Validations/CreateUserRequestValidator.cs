namespace StudyFlow.API.Validations;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator()
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
