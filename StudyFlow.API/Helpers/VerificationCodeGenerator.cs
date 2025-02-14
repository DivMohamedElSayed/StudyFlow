namespace StudyFlow.API.Helpers;

public static class VerificationCodeGenerator
{
    public static string GenerateCode()
    {
        Random random = new();
        return random.Next(100000, 999999).ToString();
    }
}
