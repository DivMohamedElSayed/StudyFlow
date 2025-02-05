namespace StudyFlow.API.Repository.Implementations;

public class UserService(
    UserManager<ApplicationUser> userManager,
    ILogger<UserService> logger
    ) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<UserService> _logger = logger;

    public async Task<Result> UpdateThemePreferenceAsync(string id, UserThemeRequest request)
    {
        _logger.LogInformation("Theme update request received for UserId: {UserId}, Requested Theme: {Theme}", id, request);
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);
        if (!ThemeConstants.ValidThemes.Contains(request.ThemePreference.ToLower()))
            return Result.Failure(UserErrors.ThemeNotFound);
        _logger.LogInformation("Updating theme preference for UserId: {UserId} to {Theme}", id, request);
        user.ThemePreference = request.ThemePreference.ToLower();
        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
            return Result.Success();
        var error = result.Errors.First();
        _logger.LogError("Failed to update theme preference for UserId: {UserId}. Error Code: {ErrorCode}, Description: {ErrorMessage}", id, error.Code, error.Description);
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result<UserProfileResponse>> GetInfoAsync(string id)
    {
        _logger.LogInformation("Fetching user profile for user ID: {UserId}", id);
        var user = await _userManager.Users
            .Where(u => u.Id == id)
            .ProjectToType<UserProfileResponse>()
            .SingleAsync();
        _logger.LogInformation("Successfully fetched user profile for user ID: {UserId}", id);
        return Result.Success(user);
    }

    public async Task<Result> UpdateInfoAsync(string id, UserProfileRequest request)
    {
        _logger.LogInformation("Updating user profile for user ID: {UserId}", id);
        var user = await _userManager.Users
            .Where(u => u.Id == id)
            .ExecuteUpdateAsync(setter =>
                setter
                .SetProperty(f => f.FirstName, request.FirstName)
                .SetProperty(l => l.LastName, request.LastName)
            );
        _logger.LogInformation("Successfully updated user profile for user ID: {UserId}", id);
        return Result.Success();
    }
}