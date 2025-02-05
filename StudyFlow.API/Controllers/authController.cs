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
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("SignIn: Received sign-in request for userName: {UserName}", request.UserName);
        var result = await _authService.SignInAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
