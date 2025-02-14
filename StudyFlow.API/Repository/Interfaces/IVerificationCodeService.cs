namespace StudyFlow.API.Repository.Interfaces;

public interface IVerificationCodeService
{
    Task StoreCodeAsync(string email, string code);
    Task<VerificationCode?> GetStoredCodeAsync(string email);
    Task MarkCodeAsUsedAsync(string email, string code);
}
