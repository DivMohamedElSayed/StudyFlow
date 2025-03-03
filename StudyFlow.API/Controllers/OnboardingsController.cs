namespace StudyFlow.API.Controllers;
[Route("onboarding")]
[ApiController]
[Authorize]
public class OnboardingsController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
}
