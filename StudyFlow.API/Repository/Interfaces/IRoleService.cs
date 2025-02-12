namespace StudyFlow.API.Repository.Interfaces;

public interface IRoleService
{
    Task<Result<RoleDetailResponse>> GetAsync(string id);
    Task<Result> ToggleStatusAsync(string id);
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
}
