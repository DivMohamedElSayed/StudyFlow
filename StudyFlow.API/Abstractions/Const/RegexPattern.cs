namespace StudyFlow.API.Abstractions.Const;

public static class RegexPattern
{
    public const string Password = "(?=(.*[0-9]))(?=.*[\\!@#$%^&*()\\\\[\\]{}\\-_+=~`|:;\"'<>,./?])(?=.*[a-z])(?=(.*[A-Z]))(?=(.*)).{8,}";
    public const string UserName = "^[a-z0-9]+([._]?[a-z0-9]+)*$";
}