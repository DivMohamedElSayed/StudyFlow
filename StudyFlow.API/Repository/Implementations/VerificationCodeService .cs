namespace StudyFlow.API.Repository.Implementations;

public class VerificationCodeService(ApplicationDbContext context) : IVerificationCodeService
{
    private const int _expirationMinutes = 10;
    private readonly ApplicationDbContext _context = context;

    public async Task StoreCodeAsync(string email, string code)
    {
        var verificationCode = new VerificationCode
        {
            Email = email,
            Code = code,
            CreatedAt = DateTime.UtcNow,
            IsUsed = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_expirationMinutes)
        };
        await _context.AddAsync(verificationCode);
        await _context.SaveChangesAsync();
    }

    public async Task<VerificationCode?> GetStoredCodeAsync(string email) =>
        await _context.VerificationCodes
        .Where(vc => vc.Email == email && !vc.IsUsed && vc.ExpiresAt > DateTime.UtcNow)
        .OrderByDescending(vc => vc.CreatedAt)
        .FirstOrDefaultAsync();
    public async Task MarkCodeAsUsedAsync(string email, string code)
    {
        var verificationCode = await _context.VerificationCodes
           .Where(vc => vc.Email == email && vc.Code == code)
           .FirstOrDefaultAsync();

        if (verificationCode != null)
        {
            verificationCode.IsUsed = true;
            await _context.SaveChangesAsync();
        }
    }
}
