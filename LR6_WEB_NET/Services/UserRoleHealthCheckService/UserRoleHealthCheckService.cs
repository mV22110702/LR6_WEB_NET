using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Services.UserRoleService;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService;

public class UserRoleHealthCheckService : IHealthCheck
{
    private readonly IUserRoleService _userRoleService;

    public UserRoleHealthCheckService(IUserRoleService _userRoleService)
    {
        this._userRoleService = _userRoleService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Log.Warning("Checking user role service health");
        var errorMessage = await _userRoleService.CheckServiceConnection();
        if (!errorMessage.IsNullOrEmpty())
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus, errorMessage);
        }
        return HealthCheckResult.Healthy("User role service is available");
    }
}