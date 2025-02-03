namespace StudyFlow.API.Repository.Implementations;

public class AuthService(
        UserManager<ApplicationUser> userManager,
        ILogger<AuthService> logger
    ) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<Result> SignUpAsync(SignUpRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting user signup process for Email: {Email}, Username: {Username}", request.Email, request.UserName);
        // Check if email already exists
        var emailExists = await _userManager.Users.AnyAsync(e => e.Email == request.Email, cancellationToken);
        if (emailExists)
        {
            _logger.LogWarning("Signup failed: Email '{Email}' is already registered.", request.Email);
            return Result.Failure(UserErrors.DuplicatedEmail);
        }

        // Check if username already exists
        var userNameExists = await _userManager.Users.AnyAsync(u => u.UserName == request.UserName, cancellationToken);
        if (userNameExists)
        {
            _logger.LogWarning("Signup failed: Username '{Username}' is already taken.", request.UserName);
            return Result.Failure(UserErrors.DuplicatedUserName);
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
            return Result.Success();
        }

        // Log the first error returned from identity
        var error = result.Errors.First();
        _logger.LogError("Signup failed for Email: {Email}, Username: {Username}. Error: {ErrorCode} - {ErrorDescription}",
            request.Email, request.UserName, error.Code, error.Description);

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}