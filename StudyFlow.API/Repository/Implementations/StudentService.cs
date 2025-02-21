namespace StudyFlow.API.Repository.Implementations;

public class StudentService(
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext context,
    ILogger<StudentService> logger
    ) : IStudentService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly ApplicationDbContext _context = context;
    private readonly ILogger<StudentService> _logger = logger;

    public async Task<Result> CreateAsync(string userId, CreateStudentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting student creation for UserId: {UserId}", userId);

        var userExists = await _userManager.Users.AnyAsync(x => x.Id == userId, cancellationToken);
        if (!userExists)
        {
            _logger.LogWarning("User not found. UserId: {UserId}", userId);
            return Result.Failure(UserErrors.UserNotFound);
        }

        var studentExists = await _context.Students.AnyAsync(x => x.UserId == userId, cancellationToken);
        if (studentExists)
        {
            _logger.LogWarning("Duplicate student creation attempt. UserId: {UserId}", userId);
            return Result.Failure(StudentErrors.DuplicatedStudent);
        }

        var student = request.Adapt<Student>();
        student.UserId = userId;

        await _context.Students.AddAsync(student, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Student created successfully. UserId: {UserId}, StudentId: {StudentId}", userId, student.Id);
        return Result.Success();
    }
    public async Task<Result<StudentResponse>> GetAsync(string userId,string id ,CancellationToken cancellationToken = default)
    {
        var result = await _context.Users
            .AsNoTracking()
            .Include(x => x.Student)
            .Where(x=>x.Id == userId)
            .Select(u => new StudentResponse(
                u.FirstName!,
                u.LastName!,
                u.UserName!,
                u.PhoneNumber!,
                u.Student.GradeLevel,
                u.Student.SchoolName,
                u.Student.PreferredSubjects.ToList()
            ))
            .SingleOrDefaultAsync(cancellationToken);
        return Result.Success(result!);
    }
}
