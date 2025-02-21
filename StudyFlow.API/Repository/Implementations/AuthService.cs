namespace StudyFlow.API.Repository.Implementations;

public class AuthService(
        UserManager<ApplicationUser> userManager,
        ILogger<AuthService> logger,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider,
        IOptions<GoogleOptions> options,
        IEmailSender emailSender,
        IVerificationCodeService verificationCode,
        ApplicationDbContext context
    ) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IVerificationCodeService _verificationCode = verificationCode;
    private readonly ApplicationDbContext _context = context;
    private readonly GoogleOptions _options = options.Value;
    private readonly int _refreshTokenExpiryDay = 30;

    public async Task<Result<AuthResponseSignIn>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Attempting to sign in with username: {Username}", request.UserName);

        // Find the user by username instead of email
        if (await _userManager.FindByNameAsync(request.UserName) is not { } user)
            return Result.Failure<AuthResponseSignIn>(UserErrors.InvalidCredentials);

        if (user.IsDisabled)
            return Result.Failure<AuthResponseSignIn>(UserErrors.UserIsDisabled);

        _logger.LogInformation("User account is active. Attempting password sign-in for username: {Username}", request.UserName);

        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            _logger.LogInformation("Sign-in succeeded for user: {Username}", request.UserName);
            var userRoles = await GetUserRoles(user,cancellationToken);
            var (token, expireIn) = _jwtProvider.GenerateToken(user,userRoles);
            var refreshTokenCode = GenerateRefreshToken();
            var refreshTokenExpirations = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);
            user.RefreshTokens.Add(new RefreshToken
            {
                RefreshTokenCode = refreshTokenCode,
                ExpireOn = refreshTokenExpirations
            });
            await _userManager.UpdateAsync(user);
            var response = new AuthResponseSignIn(user.Id,user.FirstName!,user.LastName!,user.Email, user.UserName, token, expireIn, refreshTokenCode, refreshTokenExpirations);
            return Result.Success(response,Message.SignInSuccess);
        }

        var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
            ? UserErrors.LockedOut
            : UserErrors.InvalidCredentials;

        return Result.Failure<AuthResponseSignIn>(error);
    }

    public async Task<Result<AuthResponseSignUp>> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting user signup process for Email: {Email}, Username: {Username}", request.Email, request.UserName);
        // Check if email already exists
        var emailExists = await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken);
        if (emailExists)
        {
            _logger.LogWarning("Signup failed: Email '{Email}' is already registered.", request.Email);
            return Result.Failure<AuthResponseSignUp>(UserErrors.DuplicatedEmail);
        }

        // Check if username already exists
        var userNameExists = await _userManager.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken);
        if (userNameExists)
        {
            _logger.LogWarning("Signup failed: Username '{Username}' is already taken.", request.UserName);
            return Result.Failure<AuthResponseSignUp>(UserErrors.DuplicatedUserName);
        }
        // Map request to ApplicationUser
        var user = request.Adapt<ApplicationUser>();
        var result = await _userManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            // Generate code
            var code = VerificationCodeGenerator.GenerateCode();
            await _verificationCode.StoreCodeAsync(user.Email!, code);
            _logger.LogInformation("User '{Username}' created successfully. code: {code}", user.UserName, code);
            // TODO: Implement email sending logic here
            await SendConfirmationEmail(user, code);
            var userRoles = await GetUserRoles(user, cancellationToken);
            var (token, expireIn) = _jwtProvider.GenerateToken(user, userRoles);
            var refreshTokenCode = GenerateRefreshToken();
            var refreshTokenExpirations = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);
            user.RefreshTokens.Add(new RefreshToken
            {
                RefreshTokenCode = refreshTokenCode,
                ExpireOn = refreshTokenExpirations
            });
            await _userManager.UpdateAsync(user);
            var response = new AuthResponseSignUp(user.Id, token, expireIn, refreshTokenCode, refreshTokenExpirations);
            return Result.Success(response,Message.SignUpSuccess);
        }

        // Log the first error returned from identity
        var error = result.Errors.First();

        return Result.Failure<AuthResponseSignUp>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result<AuthResponseSignIn>> RegenerateRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting to regenerate refresh token for user with access token.");
        // Validate JWT access token
        if (_jwtProvider.ValidateToken(request.AccessToken) is not { } userId)
            return Result.Failure<AuthResponseSignIn>(UserErrors.InvalidJwtToken);

        // Fetch user
        if( await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure<AuthResponseSignIn>(UserErrors.UserNotFound);

        // Check if the user is disabled or locked
        if (user.IsDisabled)
            return Result.Failure<AuthResponseSignIn>(UserErrors.UserIsDisabled);
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure<AuthResponseSignIn>(UserErrors.LockedOut);

        // Retrieve the active refresh token for the access token
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt =>
            rt.RefreshTokenCode == request.RefreshToken &&
            rt.IsActive
        );

        if (userRefreshToken is null)
            return Result.Failure<AuthResponseSignIn>(UserErrors.InvalidRefreshToken);

        // Ensure refresh token has not expired
        if (userRefreshToken.IsExpired)
            return Result.Failure<AuthResponseSignIn>(UserErrors.ExpiredRefreshToken);

        // Revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        _logger.LogInformation("Refresh token revoked for user {UserId}.", userId);

        // Generate new tokens
        var userRoles = await GetUserRoles(user, cancellationToken);
        var (newAccessToken, newExpireIn) = _jwtProvider.GenerateToken(user, userRoles);
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
        var response = new AuthResponseSignIn(user.Id, user.Email!,user.FirstName!,user.LastName!, user.UserName, newAccessToken, newExpireIn, newRefreshToken, newRefreshTokenExpiration);
        _logger.LogInformation("Refresh token regeneration completed successfully for user {UserId}.", userId);
        return Result.Success(response,Message.TokenRefreshSuccess);
    }
    public async Task<Result> RevokeRefreshTokenAsync(RefreshTokenRequest request,CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting to revoke refresh token for user with access token.");
        // Validate JWT access token
        if (_jwtProvider.ValidateToken(request.AccessToken) is not { } userId)
            return Result.Failure(UserErrors.InvalidJwtToken);

        // Fetch user
        if (await _userManager.FindByIdAsync(userId) is not { } user)
            return Result.Failure(UserErrors.UserNotFound);

        // Check if the user is disabled or locked
        if (user.IsDisabled)
            return Result.Failure(UserErrors.UserIsDisabled);
        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow)
            return Result.Failure(UserErrors.LockedOut);

        // Retrieve the active refresh token for the access token
        var userRefreshToken = user.RefreshTokens.SingleOrDefault(rt =>
            rt.RefreshTokenCode == request.RefreshToken &&
            rt.IsActive
        );

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        // Ensure refresh token has not expired
        if (userRefreshToken.IsExpired)
            return Result.Failure(UserErrors.ExpiredRefreshToken);

        // Revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;
        _logger.LogInformation("Refresh token revoked for user {UserId}.", userId);
        await _userManager.UpdateAsync(user);
        return Result.Success();
    }
    public async Task<Result> SendForgetPasswordCodeAsync(ForgetPasswordRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success(""); // To the hacker  
        if(!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);
        var accessToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        accessToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(accessToken));
        _logger.LogInformation("Reset Access Token: {accessToken}",accessToken);
        // TODO: Send Reset Password Email in BackGround Job
        return Result.Success();
    }
    public async Task<Result> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);
        IdentityResult result;
        try
        {
            var accessToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.AccessToken));
            result = await _userManager.ResetPasswordAsync(user, accessToken, request.NewPassword);
        }
        catch (FormatException)
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }

        if(result.Succeeded)
            return Result.Success();
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        var storedCode = await _verificationCode.GetStoredCodeAsync(user.Email!);
        if (storedCode is null || storedCode.Code != request.Code)
            return Result.Failure(UserErrors.InvalidCode);
        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        _logger.LogInformation("Token: {token}", token);
        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            await _verificationCode.MarkCodeAsUsedAsync(user.Email!, request.Code);
            await _context.SaveChangesAsync();
            await _userManager.AddToRoleAsync(user, DefaultRoles.Student);
            return Result.Success();
        }
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
    public async Task<Result<AuthResponseSignIn>> GoogleSignInAsync(GoogleSignInRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var payload = await ValidateGoogleTokenAsync(request.GoogleToken);
            if (payload == null)
            {
                return Result.Failure<AuthResponseSignIn>(UserErrors.InvalidCode);
            }

            // Find or create user
            var user = await GetOrCreateGoogleUserAsync(payload);

            // Generate JWT and refresh token
            return await GenerateAuthResponseAsync(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google authentication failed");
            return Result.Failure<AuthResponseSignIn>(UserErrors.GoogleAuthFailed);
        }
    }
    private async Task<ApplicationUser> GetOrCreateGoogleUserAsync(GoogleJsonWebSignature.Payload payload)
    {
        // First try to find user by Google ID
        var user = await _userManager.Users
            .FirstOrDefaultAsync(u =>
                u.Provider == AuthProvider.Google &&
                u.ExternalProviderId == payload.Subject);

        // If not found, try by email
        if (user == null)
        {
            user = await _userManager.FindByEmailAsync(payload.Email);

            if (user != null && user.Provider != AuthProvider.Google)
            {
                _logger.LogWarning("User {Email} tried to sign in with Google but already exists with {Provider}",
                    payload.Email, user.Provider);
                throw new InvalidOperationException("User already exists with different provider");
            }
        }

        // If still no user, create new one
        if (user == null)
        {
            user = new ApplicationUser
            {
                Email = payload.Email,
                UserName = GenerateUsername(payload.Email),
                EmailConfirmed = true,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Provider = AuthProvider.Google,
                ExternalProviderId = payload.Subject
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                _logger.LogError("Failed to create user from Google authentication: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
                throw new ApplicationException("Failed to create user");
            }

            await _userManager.AddToRoleAsync(user, "User");
        }

        return user;
    }
    private static string GenerateUsername(string email) =>
      $"{email.Split('@')[0]}{DateTime.UtcNow.Ticks % 1000}";
    private async Task<Result<AuthResponseSignIn>> GenerateAuthResponseAsync(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, expireIn) = _jwtProvider.GenerateToken(user, roles);

        // Generate refresh token
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);

        // Add refresh token to user
        user.RefreshTokens.Add(new RefreshToken
        {
            RefreshTokenCode = refreshToken,
            ExpireOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        return Result.Success(new AuthResponseSignIn(
            user.Id,
            user.FirstName!,
            user.LastName!,
            user.Email,
            user.UserName,
            accessToken,
            expireIn,
            refreshToken,
            refreshTokenExpiration
        ));
    }
    private async Task<GoogleJsonWebSignature.Payload> ValidateGoogleTokenAsync(string token)
    {
        var settings = new GoogleJsonWebSignature.ValidationSettings
        {
            Audience = new[] { _options.ClientId }
        };
        return await GoogleJsonWebSignature.ValidateAsync(token, settings);
    }

    private static string GenerateRefreshToken() =>
        Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private async Task<IEnumerable<string>> GetUserRoles (ApplicationUser user,CancellationToken cancellationToken = default) =>
        await _userManager.GetRolesAsync(user);
    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
             new Dictionary<string, string>
             {
                { "{{userName}}", user.UserName! },
                    { "{{verificationCode}}",code }
             }
        );
        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Study Flow : Email Confirmation", emailBody));
        await _verificationCode.StoreCodeAsync(user.Email!,code);
        await Task.CompletedTask;
    }
}