using LR6_WEB_NET.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace LR6_WEB_NET.Services.DBSeedingHealthCheckService;

public class DBHealthCheckService : IHealthCheck
{
    private readonly DataContext _dataContext;

    public DBHealthCheckService(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = new CancellationToken())
    {
        if (!await _dataContext.Database.CanConnectAsync(cancellationToken))
        {
            return new HealthCheckResult(
                context.Registration.FailureStatus, "Database is not available");
        }
        await _dataContext.DisposeAsync();
        
        return HealthCheckResult.Healthy("Database is available");
    }
}