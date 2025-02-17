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

        RuleFor(p => p.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .WithMessage("Invalid phone number. It must start with 010, 011, 012, or 015 and contain exactly 11 digits.");

        RuleFor(d => d.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of Birth is required.")
            .Must(BeAtLeast6YearsOld)
            .WithMessage("The person must be at least 6 years old.");
    }
    private static bool BeAtLeast6YearsOld(DateOnly dateOfBirth) =>
    dateOfBirth <= DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-6));

}
