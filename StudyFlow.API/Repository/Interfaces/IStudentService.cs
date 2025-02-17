namespace StudyFlow.API.Repository.Interfaces;

public interface IStudentService
{
    Task<Result> CreateAsync(string userId, CreateStudentRequest request, CancellationToken cancellationToken = default);
}
