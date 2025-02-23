namespace StudyFlow.API.Repository.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponseSignUp>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);

    Task<Result<AuthResponseSignIn>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseSignIn>> RegenerateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> SendForgetPasswordCodeAsync(ForgetPasswordRequest request);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
    Task<Result<AuthResponseSignIn>> GoogleSignInAsync(GoogleSignInRequest request, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request);
    Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request);
}