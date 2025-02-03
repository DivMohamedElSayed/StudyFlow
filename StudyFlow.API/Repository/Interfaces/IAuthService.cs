namespace StudyFlow.API.Repository.Interfaces;

public interface IAuthService
{
    Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);

    Task<Result<SignInResponse>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default);
}