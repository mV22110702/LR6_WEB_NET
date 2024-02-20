namespace LR6_WEB_NET.Services.AuthService;

public class HealthCheckResultDto
{
    public string Status { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public IDictionary<string, string?> Data { get; set; } = new Dictionary<string, string?>();
}