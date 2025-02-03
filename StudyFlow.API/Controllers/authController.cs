namespace StudyFlow.API.Controllers;

[Route("[controller]")]
[ApiController]
public class authController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.SignUpAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
}