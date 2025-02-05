namespace StudyFlow.API.Controllers;

[Route("account")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPut("theme-preference")]
    public async Task<IActionResult> UpdateThemePreference([FromBody] UserThemeRequest request)
    {
        var result = await _userService.UpdateThemePreferenceAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await _userService.GetInfoAsync(User.GetUserId()!);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("info")]
    public async Task<IActionResult> UpdateInfo([FromBody] UserProfileRequest request)
    {
        var result = await _userService.UpdateInfoAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}