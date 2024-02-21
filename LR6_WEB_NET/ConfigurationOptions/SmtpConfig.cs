using System.Text.Json.Serialization;

namespace LR6_WEB_NET.ConfigurationOptions;

public class SmtpConfig
{
    [JsonPropertyName("sendgridApiKey")]
    public string SendGridApiKey { get; set; }
}