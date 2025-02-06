namespace StudyFlow.API.Repository.Implementations;

public class AuthService(
        UserManager<ApplicationUser> userManager,
        ILogger<AuthService> logger,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider
    ) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshTokenExpiryDay = 30;

    public async Task<Result<AuthResponse>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Attempting to sign in with username: {Username}", request.UserName);

        // Find the user by username instead of email
        if (await _userManager.FindByNameAsync(request.UserName) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserIsDisabled);

        _logger.LogInformation("User account is active. Attempting password sign-in for username: {Username}", request.UserName);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            _logger.LogInformation("Sign-in succeeded for user: {Username}", request.UserName);
            var (token, expireIn) = _jwtProvider.GenerateToken(user);
            var refreshTokenCode = GenerateRefreshToken();
            var refreshTokenExpirations = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);
            user.RefreshTokens.Add(new RefreshToken
            {
                RefreshTokenCode = refreshTokenCode,
                ExpireOn = refreshTokenExpirations
            });
            await _userManager.UpdateAsync(user);
            var response = new AuthResponse(user.Id, user.Email, user.UserName, token, expireIn, refreshTokenCode, refreshTokenExpirations);
            return Result.Success(response);
        }

        var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
            ? UserErrors.LockedOut
            : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponse>(error);
    }

    public async Task<Result<AuthResponse>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting user signup process for Email: {Email}, Username: {Username}", request.Email, request.UserName);
        // Check if email already exists
        var emailExists = await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken);
        if (emailExists)
        {
            _logger.LogWarning("Signup failed: Email '{Email}' is already registered.", request.Email);
            return Result.Failure<AuthResponse>(UserErrors.DuplicatedEmail);
        }

        // Check if username already exists
        var userNameExists = await _userManager.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken);
        if (userNameExists)
        {
            _logger.LogWarning("Signup failed: Username '{Username}' is already taken.", request.UserName);
            return Result.Failure<AuthResponse>(UserErrors.DuplicatedUserName);
        }
        // Map request to ApplicationUser
        var user = request.Adapt<ApplicationUser>();
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            // Generate Token
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("User '{Username}' created successfully. Token: {code}", user.UserName, code);
            // TODO: Implement email sending logic here
            var (token, expireIn) = _jwtProvider.GenerateToken(user);
            var refreshTokenCode = GenerateRefreshToken();
            var refreshTokenExpirations = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);
            user.RefreshTokens.Add(new RefreshToken
            {
                RefreshTokenCode = refreshTokenCode,
                ExpireOn = refreshTokenExpirations
            });
            await _userManager.UpdateAsync(user);
            var response = new AuthResponse(user.Id, user.Email, user.UserName, token, expireIn, refreshTokenCode, refreshTokenExpirations);
            return Result.Success(response);
        }

        // Log the first error returned from identity
        var error = result.Errors.First();
        _logger.LogError("Signup failed for Email: {Email}, Username: {Username}. Error: {ErrorCode} - {ErrorDescription}",
            request.Email, request.UserName, error.Code, error.Description);

        return Result.Failure<AuthResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result<AuthResponse>> RegenerateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting to regenerate refresh token for user with access token.");
        // Validate JWT access token
        if (_jwtProvider.ValidateToken(request.AccessToken) is not { } userId)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        // Fetch user
        if( await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        // Check if the user is disabled or locked
        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserIsDisabled);
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedOut);

        // Retrieve the active refresh token for the access token
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt =>
            rt.RefreshTokenCode == request.RefreshToken &&
            rt.IsActive
        );

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        // Ensure refresh token has not expired
        if (userRefreshToken.IsExpired)
            return Result.Failure<AuthResponse>(UserErrors.ExpiredRefreshToken);

        // Revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        _logger.LogInformation("Refresh token revoked for user {UserId}.", userId);

        // Generate new tokens
        var (newAccessToken, newExpireIn) = _jwtProvider.GenerateToken(user);
        var newRefreshToken = GenerateRefreshToken();
        var newRefreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);

        // Add new refresh token
        user.RefreshTokens.Add(new RefreshToken
        {
            RefreshTokenCode = newRefreshToken,  // Store refresh token separately
            ExpireOn = newRefreshTokenExpiration,
        });

        // Update user in database
        await _userManager.UpdateAsync(user);
        _logger.LogInformation("New refresh token generated and user {UserId} updated in the database.", userId);
        // Return response
        var response = new AuthResponse(user.Id, user.Email, user.UserName, newAccessToken, newExpireIn, newRefreshToken, newRefreshTokenExpiration);
        _logger.LogInformation("Refresh token regeneration completed successfully for user {UserId}.", userId);
        return Result.Success(response);
    }
    public async Task<Result> RevokeRefreshTokenAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting to revoke refresh token for user with access token.");
        // Validate JWT access token
        if (_jwtProvider.ValidateToken(request.AccessToken) is not { } userId)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        // Fetch user
        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponse>(UserErrors.UserNotFound);

        // Check if the user is disabled or locked
        if (user.IsDisabled)
            return Result.Failure<AuthResponse>(UserErrors.UserIsDisabled);
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponse>(UserErrors.LockedOut);

        // Retrieve the active refresh token for the access token
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt =>
            rt.RefreshTokenCode == request.RefreshToken &&
            rt.IsActive
        );

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        // Ensure refresh token has not expired
        if (userRefreshToken.IsExpired)
            return Result.Failure<AuthResponse>(UserErrors.ExpiredRefreshToken);

        // Revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        _logger.LogInformation("Refresh token revoked for user {UserId}.", userId);
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }
    private static string GenerateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}