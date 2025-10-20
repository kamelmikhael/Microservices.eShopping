using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Ordering.Infrastructure.Services;

internal sealed class EmailService(
    IOptions<EmailSettings> emailSettings
    , ILogger<EmailService> logger) : IEmailService
{
    private readonly EmailSettings _emailSettings = emailSettings.Value;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task<bool> SendAsync(Email email)
    {
        var client = new SendGridClient(_emailSettings.ApiKey);

        var sendGridMessage = MailHelper.CreateSingleEmail(
            new(_emailSettings.FromAddress, _emailSettings.FromName),
            new(email.To),
            email.Subject,
            email.Body,
            email.Body);

        var response = await client.SendEmailAsync(sendGridMessage);

        _logger.LogInformation("Email sent successfuly");

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted
            || response.StatusCode == System.Net.HttpStatusCode.OK)
            return true;

        _logger.LogError("Email Sent Failed");
        
        return false;
    }
}
