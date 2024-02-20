using System.Text;
using System.Text.Json;
using LR6_WEB_NET.Models.Dto;
using LR6_WEB_NET.Services.AuthService;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService
{
    public static class WriteHealthCheckResponse
    {
        public static async Task WriteJsonResponse(HttpContext context, HealthReport healthReport)
        {
            context.Response.ContentType = "application/json";
            var jsonOptions = new JsonOptions() { SerializerOptions = { WriteIndented = true } };
            IDictionary<string, HealthCheckResultDto> healthCheckResults =
                new Dictionary<string, HealthCheckResultDto>();
            foreach (var healthReportEntry in healthReport.Entries)
            {
                var entryKey = healthReportEntry.Key;
                var entryValue = new HealthCheckResultDto()
                {
                    Status = healthReportEntry.Value.Status.ToString(),
                    Description = healthReportEntry.Value.Description,
                };
                foreach (var item in healthReportEntry.Value.Data)
                {
                    entryValue.Data.Add(item.Key, item.Value.ToString());
                }
                healthCheckResults.Add(entryKey, entryValue);
            }

            var healthStatusToStatusCodes = new Dictionary<HealthStatus, int>
            {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status200OK,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
            };

            var response = new HealthCheckResponseDto()
            {
                Description = "Health check results",
                StatusCode = healthStatusToStatusCodes[healthReport.Status],
                Results = healthCheckResults
            };
            
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}