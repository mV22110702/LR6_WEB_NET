using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Services.ShiftService;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;

namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService;

public class ShiftHealthCheckService : IHealthCheck
{
    private readonly IShiftService _shiftService;

    public ShiftHealthCheckService(IShiftService _shiftService)
    {
        this._shiftService = _shiftService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var errorMessage = await _shiftService.CheckServiceConnection();
        if (!errorMessage.IsNullOrEmpty())
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus, errorMessage);
        }
        return HealthCheckResult.Healthy("Shift service is available");
    }
}