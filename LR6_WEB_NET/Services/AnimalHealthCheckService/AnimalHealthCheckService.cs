using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService;

public class AnimalHealthCheckService : IHealthCheck
{
    private readonly IAnimalService _animalService;

    public AnimalHealthCheckService(IAnimalService _animalService)
    {
        this._animalService = _animalService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        Log.Warning("Checking animal service health");
        var errorMessage = await _animalService.CheckServiceConnection();
        if (!errorMessage.IsNullOrEmpty())
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus, errorMessage);
        }
        return HealthCheckResult.Healthy("Animal service is available");
    }
}