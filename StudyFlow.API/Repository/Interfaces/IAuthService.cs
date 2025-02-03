namespace StudyFlow.API.Repository.Interfaces;

public interface IAuthService
{
    Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default);
}