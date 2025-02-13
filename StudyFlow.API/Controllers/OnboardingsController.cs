namespace StudyFlow.API.Controllers;
[Route("onboarding")]
[ApiController]
[Authorize]
public class OnboardingsController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpPut("general")]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        var result = await _userService.CreateAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
    [HttpPut("general/role")]
    public async Task<IActionResult> Create([FromBody] CreateUserRoleRequest request)
    {
        var result = await _userService.CreateAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
