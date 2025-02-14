namespace StudyFlow.API.Controllers;
[Route("auth")]
[ApiController]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SignUp: Received sign-up request for email: {Email}, username: {Username}", request.Email, request.UserName);
        var result = await _authService.SignUpAsync(request, cancellationToken);
        return result.IsSuccess ? result.ToResponse() : result.ToProblem();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SignIn: Received sign-in request for userName: {UserName}", request.UserName);
        var result = await _authService.SignInAsync(request, cancellationToken);
        return result.IsSuccess ? result.ToResponse() : result.ToProblem();
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> GetRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegenerateRefreshTokenAsync(request, cancellationToken);
        return result.IsSuccess ? result.ToResponse() : result.ToProblem();
    }
    [HttpPut("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request,CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request,cancellationToken);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody]  ForgetPasswordRequest request)
    {
        var result = await _authService.SendForgetPasswordCodeAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);
        return result.IsSuccess? Ok() : result.ToProblem();
    }
    [HttpPost("signin-google")]
    public async Task<IActionResult> GoogleSignIn([FromBody] GoogleSignInRequest request,CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received Google sign-in request");
        var result = await _authService.GoogleSignInAsync(request,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("verify-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
    {
        var result = await _authService.ConfirmEmailAsync(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}
