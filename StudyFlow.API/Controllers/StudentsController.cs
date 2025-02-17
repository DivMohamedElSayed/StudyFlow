namespace StudyFlow.API.Controllers;
[Route("users/students")]
[ApiController]
[Authorize]
public class StudentsController(IStudentService studentService) : ControllerBase
{
    private readonly IStudentService _studentService = studentService;
    [HttpPost("")]
    public async Task<IActionResult> Create([FromBody] CreateStudentRequest request,CancellationToken cancellationToken)
    {
        var result = await _studentService.CreateAsync(User.GetUserId()!, request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
}
