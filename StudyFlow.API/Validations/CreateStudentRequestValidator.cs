namespace StudyFlow.API.Validations;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    public CreateStudentRequestValidator()
    {
        RuleFor(g => g.GradeLevel)
            .NotEmpty()
            .WithMessage("Grade level is required.");

        RuleFor(p => p.ParentPhoneNumber)
            .NotEmpty()
            .WithMessage("Parent phone number is required.");

        RuleFor(s => s.SchoolName)
            .NotEmpty()
            .WithMessage("School name is required.");

        RuleFor(p => p.PreferredSubjects)
            .NotNull()
            .WithMessage("Preferred subjects must be provided.")
            .NotEmpty()
            .WithMessage("Please select at least one preferred subject.");
    }
}
