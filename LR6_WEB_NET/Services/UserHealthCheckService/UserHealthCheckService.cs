using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService;

public class UserHealthCheckService : IHealthCheck
{
    private readonly IUserService _userService;

    public UserHealthCheckService(IUserService _userService)
    {
        this._userService = _userService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Log.Warning("Checking user service health");
        var errorMessage = await _userService.CheckServiceConnection();
        if (!errorMessage.IsNullOrEmpty())
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus, errorMessage);
        }
        return HealthCheckResult.Healthy("User service is available");
    }
}