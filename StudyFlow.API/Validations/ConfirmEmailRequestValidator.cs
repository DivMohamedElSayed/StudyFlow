namespace StudyFlow.API.Validations;

public class ConfirmEmailRequestValidator : AbstractValidator<ConfirmEmailRequest>
{
    public ConfirmEmailRequestValidator()
    {
        RuleFor(u => u.UserId)
            .NotEmpty()
            .WithMessage("userId is required.");
        RuleFor(c => c.Code)
            .NotEmpty()
            .WithMessage("code is required.");
    }
}
