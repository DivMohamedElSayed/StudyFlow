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

    public async Task<Result<SignInResponse>> SignInAsync(SignInRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Attempting to sign in with email: {Email}", request.Email);
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Failure<SignInResponse>(UserErrors.InvalidCredentials);
        if (user.IsDisabled)
            return Result.Failure<SignInResponse>(UserErrors.UserIsDisabled);
        _logger.LogInformation("User account is active. Attempting password sign-in for email: {Email}", request.Email);
        var result = await _signInManager.PasswordSignInAsync(user, request.Password, false, true);
        if (result.Succeeded)
        {
            _logger.LogInformation("Sign-in succeeded for user: {Email}", request.Email);
            var (token, expireIn) = _jwtProvider.GenerateToken(user);
            await _userManager.UpdateAsync(user);
            var response = new SignInResponse(user.Id, user.FirstName, user.LastName, user.Email, user.UserName, token, expireIn);
            return Result.Success(response);
        }
        var error = result.IsNotAllowed
            ? UserErrors.EmailNotConfirmed
            : result.IsLockedOut
            ? UserErrors.LockedOut
            : UserErrors.InvalidCredentials;

        return Result.Failure<SignInResponse>(error);
    }

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