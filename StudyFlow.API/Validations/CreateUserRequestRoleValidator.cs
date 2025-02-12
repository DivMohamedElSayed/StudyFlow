namespace StudyFlow.API.Validations;

public class CreateUserRequestRoleValidator : AbstractValidator<CreateUserRequestRole>
{
    public CreateUserRequestRoleValidator()
    {
        RuleFor(x => x.Roles)
            .Must(x => x.Distinct().Count() == x.Count)
            .When(x => x.Roles != null);
    }
}
