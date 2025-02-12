namespace StudyFlow.API.Repository.Implementations;

public class EmailService(IOptions<MailSetting> options, ILogger<EmailService> logger) : IEmailSender
{
    private readonly MailSetting _options = options.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var message = new MimeMessage
        {
            Sender = MailboxAddress.Parse(_options.Mail),
            Subject = subject
        };
        message.To.Add(MailboxAddress.Parse(email));
        var builder = new BodyBuilder
        {
            HtmlBody = htmlMessage
        };
        message.Body = builder.ToMessageBody();
        using var smtp = new SmtpClient();
        _logger.LogInformation("Sending Email To {Email}", email);
        smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_options.Mail, _options.Password);
        await smtp.SendAsync(message);
        smtp.Disconnect(true);
    }
}
