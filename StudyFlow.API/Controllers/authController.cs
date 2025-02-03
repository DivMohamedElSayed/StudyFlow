namespace StudyFlow.API.Controllers;

[Route("[controller]")]
[ApiController]
public class authController(IAuthService authService, ILogger<authController> logger) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ILogger<authController> _logger = logger;

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received sign-up request for email: {Email}", request.Email);
        var result = await _authService.SignUpAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received sign-in request for email: {Email}", request.Email);
        var result = await _authService.SignInAsync(request, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}