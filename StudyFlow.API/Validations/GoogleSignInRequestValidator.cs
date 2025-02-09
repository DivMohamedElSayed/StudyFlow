namespace StudyFlow.API.Validations;

public class GoogleSignInRequestValidator : AbstractValidator<GoogleSignInRequest>
{
    public GoogleSignInRequestValidator()
    {
        RuleFor(g => g.GoogleToken)
            .NotEmpty()
            .WithMessage("GoogleToken is required.");
    }
}
