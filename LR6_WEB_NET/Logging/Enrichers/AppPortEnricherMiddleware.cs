using Microsoft.AspNetCore.Hosting.Server.Features;
using Serilog;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace LR6_WEB_NET.Logging.Enrichers;

class AppPortEnricherMiddleware
{
    private readonly RequestDelegate _next;

    public AppPortEnricherMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IConfiguration configuration, IServerAddressesFeature serverAddressesFeature)
    {
        Log.Warning("HERE");
        var url = configuration["Kestrel:Endpoints:Http:Url"];
        if (string.IsNullOrEmpty(url))
        {
            await _next.Invoke(context);
            return;
        }
        var port = new Uri(url).Port;
        using (LogContext.PushProperty("AppPort", port))
        {
            await _next.Invoke(context);
        }
    }
}