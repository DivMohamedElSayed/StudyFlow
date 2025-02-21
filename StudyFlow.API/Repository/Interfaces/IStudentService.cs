namespace StudyFlow.API.Repository.Interfaces;

public interface IStudentService
{
    Task<Result> CreateAsync(string userId, CreateStudentRequest request, CancellationToken cancellationToken = default);
    Task<Result<StudentResponse>> GetAsync(string userId, string id, CancellationToken cancellationToken = default);
}
