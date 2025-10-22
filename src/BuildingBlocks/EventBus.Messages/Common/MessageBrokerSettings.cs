using System.ComponentModel.DataAnnotations;

namespace EventBus.Messages.Common;

public class MessageBrokerSettings
{
    [Required]
    public string Host { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }
}
