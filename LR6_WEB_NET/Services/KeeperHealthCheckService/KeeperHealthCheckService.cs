using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Services.KeeperService;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService;

public class KeeperHealthCheckService : IHealthCheck
{
    private readonly IKeeperService _keeperService;

    public KeeperHealthCheckService(IKeeperService _keeperService)
    {
        this._keeperService = _keeperService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Log.Warning("Checking keeper service health");
        var errorMessage = await _keeperService.CheckServiceConnection();
        if (!errorMessage.IsNullOrEmpty())
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus, errorMessage);
        }
        return HealthCheckResult.Healthy("Keeper service is available");
    }
}