namespace StudyFlow.API.Repository.Interfaces;

public interface IJwtProvider
{
    (string token, int expireIn) GenerateToken(ApplicationUser user,IEnumerable<string> roles);
    string? ValidateToken(string token);
}