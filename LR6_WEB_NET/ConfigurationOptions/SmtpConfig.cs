using System.Text.Json.Serialization;

namespace LR6_WEB_NET.ConfigurationOptions;

public class SmtpConfig
{
    [JsonRequired]
    [JsonPropertyName("sendgridApiKey")]
    public string SendGridApiKey { get; set; }

    [JsonRequired]
    [JsonPropertyName("host")]
    public string Host { get; set; }

    [JsonRequired]
    [JsonPropertyName("port")]
    public int Port { get; set; }

    [JsonRequired]
    [JsonPropertyName("from")]
    public string From { get; set; }

    [JsonRequired]
    [JsonPropertyName("to")]
    public string To { get; set; }
}