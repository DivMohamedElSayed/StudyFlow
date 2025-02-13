namespace StudyFlow.API.Validations;

public class CreateUserRoleRequestValidator : AbstractValidator<CreateUserRoleRequest>
{
    public CreateUserRoleRequestValidator()
    {
        RuleFor(r => r.Roles)
            .Must(r => r.Distinct().Count() == r.Count)
            .WithMessage("you cannot add dublicated roles for the same role")
            .When(r => r.Roles != null);
    }
}
