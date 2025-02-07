namespace StudyFlow.API.Repository.Interfaces;

public interface IAuthService
{
    Task<Result<AuthResponse>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);

    Task<Result<AuthResponse>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponse>> RegenerateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    Task<Result> SendForgetPasswordCodeAsync(ForgetPasswordRequest request);
    Task<Result> ResetPasswordAsync(ResetPasswordRequest request);
}