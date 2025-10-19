namespace Ordering.Application.Contracts.Infrastructure;

public interface IEmailService
{
    Task SendAsync(Email email);
}

public class Email
{
    public string To { get; set; } = default!;
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}

public class EmailSettings
{
    public string ApiKey { get; set; } = default!;
    public string FromAddress { get; set; } = default!;
    public string FromName { get; set; } = default!;
    public string SmtpServer { get; set; } = default!;
    public int SmtpPort { get; set; }
    public string SmtpUsername { get; set; } = default!;
    public string SmtpPassword { get; set; } = default!;
}
