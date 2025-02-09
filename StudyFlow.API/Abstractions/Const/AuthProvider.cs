namespace StudyFlow.API.Abstractions.Const;

public static class AuthProvider
{
    public const string Google = "google";
    public const string Facebook = "facebook";
    public const string Local = "local";
    public static readonly string[] ValidThemes = { Google, Facebook, Local };
}
