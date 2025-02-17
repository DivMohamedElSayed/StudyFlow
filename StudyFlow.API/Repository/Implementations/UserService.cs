namespace StudyFlow.API.Repository.Implementations;

public class UserService(
    UserManager<ApplicationUser> userManager,
    ILogger<UserService> logger,
    ApplicationDbContext context
    ) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<UserService> _logger = logger;
    private readonly ApplicationDbContext _context = context;

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
        return Result.Success(user,Message.UserProfileSuccess);
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
    public async Task<Result> ChangePasswordAsync(string id,ChangePasswordRequest request)
    {
        _logger.LogInformation("Fetching user profile for user ID: {UserId}", id);
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);
        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (result.Succeeded)
            return Result.Success();
        _logger.LogInformation("Successfully change password user profile for user ID: {UserId},and the new Password: {newPassword}", id,request.NewPassword);
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await (from user in _context.Users
               join userRole in _context.UserRoles
               on user.Id equals userRole.UserId
               join role in _context.Roles
               on userRole.RoleId equals role.Id into roles
               //where !roles.Any(x => x.Name == DefaultRoles.Student)
               select new
               {
                   user.Id,
                   user.FirstName,
                   user.LastName,
                   user.Email,
                   user.IsDisabled,
                   Roles = roles.Select(x => x.Name!).ToList()
               })
                .GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })
                .Select(u => new UserResponse(
                u.Key.Id,
                u.Key.FirstName,
                u.Key.LastName,
                u.Key.Email,
                u.Key.IsDisabled,
                u.SelectMany(x => x.Roles)
                ))
                .ToListAsync(cancellationToken);
    public async Task<Result<UserResponse>> GetAsync(string id)
    {
        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure<UserResponse>(UserErrors.UserNotFound);
        var userRoles = await _userManager.GetRolesAsync(user);
        var response = (user, userRoles).Adapt<UserResponse>();
        return Result.Success(response,Message.UserSuccess);
    }
    public async Task<Result> CreateAsync(string id,CreateUserRequest request)
    {
        // Validate input
        if (string.IsNullOrEmpty(request.FirstName) ||
            string.IsNullOrEmpty(request.LastName) ||
            string.IsNullOrEmpty(request.PhoneNumber))
            return Result.Failure(UserErrors.InvalidUserData);

        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);
        user.PhoneNumber = request.PhoneNumber;
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.PhoneNumberConfirmed = true;
        user.DateOfBirth = request.DateOfBirth;
        // Create user without password (assuming password is optional)
        var result = await _userManager.UpdateAsync(user);

        if (result.Succeeded)
            return Result.Success();

        // Handle errors properly
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result> CreateAsync(string id, CreateUserRoleRequest request)
    {
        _logger.LogInformation("Received role request for user {UserId}: {Roles}", id, string.Join(", ", request.Roles ?? new List<string>()));

        if (await _userManager.FindByIdAsync(id) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        var validRoles = new[] { DefaultRoles.Student, DefaultRoles.Teacher, DefaultRoles.Parent, DefaultRoles.Guest };

        // Ensure roles are valid (case-insensitive check)
        var invalidRoles = request.Roles!.Where(role => !validRoles.Contains(role, StringComparer.OrdinalIgnoreCase)).ToList();
        if (invalidRoles.Any())
        {
            _logger.LogWarning("User {UserId} attempted to assign invalid roles: {InvalidRoles}", id, string.Join(", ", invalidRoles));
            return Result.Failure(RoleErrors.InvalidRole);
        }

        // Remove existing roles
        var existingRoles = await _userManager.GetRolesAsync(user);
        if (existingRoles.Any())
        {
            await _userManager.RemoveFromRolesAsync(user, existingRoles);
        }

        // Assign new roles
        var result = await _userManager.AddToRolesAsync(user, request.Roles!);
        if (!result.Succeeded)
        {
            var error = result.Errors.First();
            _logger.LogError("Failed to assign roles to user {UserId}. Error: {ErrorCode} - {ErrorDescription}", id, error.Code, error.Description);
            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }

        _logger.LogInformation("Successfully assigned roles to user {UserId}: {Roles}", id, string.Join(", ", request.Roles!));
        return Result.Success();
    }
}