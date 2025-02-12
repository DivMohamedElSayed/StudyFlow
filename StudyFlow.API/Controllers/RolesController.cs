namespace StudyFlow.API.Controllers;
[Route("roles")]
[ApiController]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var result =  await _roleService.GetAsync(id);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken)
    {
        var results = await _roleService.GetAllAsync(includeDisabled,cancellationToken);
        return Ok(results);
    }
    [HttpPut("toggle-status")]
    public async Task<IActionResult> ToggleStatus(string id)
    {
        var result = await _roleService.ToggleStatusAsync(id);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
