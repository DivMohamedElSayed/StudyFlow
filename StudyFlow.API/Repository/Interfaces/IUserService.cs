namespace StudyFlow.API.Repository.Interfaces;

public interface IUserService
{
    Task<Result> UpdateThemePreferenceAsync(string id, UserThemeRequest request);
    Task<Result<UserThemeResponse>> GetThemePreferencesAsync(string id);

    Task<Result<UserProfileResponse>> GetInfoAsync(string id);

    Task<Result> UpdateInfoAsync(string id, UserProfileRequest request);
    Task<Result> ChangePasswordAsync(string id, ChangePasswordRequest request);
}