namespace StudyFlow.API.Helpers;

public static class EmailBodyBuilder
{
    public static string GenerateEmailBody(string Template, Dictionary<string, string> TemplateModel)
    {
        var TemplatePath = $"{Directory.GetCurrentDirectory()}/Templates/{Template}.html";
        var StreamReader = new StreamReader(TemplatePath);
        var Body = StreamReader.ReadToEnd();
        StreamReader.Close();
        foreach (var item in TemplateModel)
            Body = Body.Replace(item.Key, item.Value);
        return Body;
    }
}
