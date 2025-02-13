namespace StudyFlow.API.Controllers;
[Route("users")]
[ApiController]
[Authorize]
public class UsersController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;
    [HttpGet("")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var results = await _userService.GetAllAsync(cancellationToken);
        return Ok(results);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var result = await _userService.GetAsync(id);
        return result.IsSuccess ? result.ToResponse() : result.ToProblem();
    }
}
