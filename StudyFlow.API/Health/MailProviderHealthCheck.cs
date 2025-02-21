namespace StudyFlow.API.Health;

public class MailProviderHealthCheck(IOptions<MailSetting> options) : IHealthCheck
{
	private readonly MailSetting _options = options.Value;

	public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
	{
		try
		{
			var smtp = new SmtpClient();
			smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls, cancellationToken);
			smtp.Authenticate(_options.Mail, _options.Password, cancellationToken);
			return await Task.FromResult(HealthCheckResult.Healthy());
		}
		catch (Exception ex)
		{
			return await Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
		} 
	}
}
