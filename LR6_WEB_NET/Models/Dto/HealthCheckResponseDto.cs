using LR6_WEB_NET.Models.Dto;

namespace LR6_WEB_NET.Services.AuthService;

public class HealthCheckResponseDto : ResponseDtoBase
{
    public IDictionary<string,HealthCheckResultDto> Results { get; set; } = new Dictionary<string, HealthCheckResultDto>();
}