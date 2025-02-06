namespace StudyFlow.API.Validations;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(a => a.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required.");

        RuleFor(r => r.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
