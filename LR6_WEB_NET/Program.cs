using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using HealthChecks.UI.Client;
using LR6_WEB_NET.ConfigurationOptions;
using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Extensions;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Enums;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.AnimalStatisticsService;
using LR6_WEB_NET.Services.AuthService;
using LR6_WEB_NET.Services.DBSeedingHealthCheckService;
using LR6_WEB_NET.Services.KeeperService;
using LR6_WEB_NET.Services.ShiftService;
using LR6_WEB_NET.Services.UserRoleService;
using LR6_WEB_NET.Services.UserService;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using Serilog.Sinks.SystemConsole.Themes;

try
{
    var builder = WebApplication.CreateBuilder(args);
    SmtpConfig? credentials = JsonSerializer.Deserialize<SmtpConfig>(File.ReadAllText("creds.json"));

    var url = builder.Configuration["Kestrel:Endpoints:Http:Url"];
    if (url.IsNullOrEmpty())
    {
        throw new Exception("Kestrel:Endpoints:Http:Url is not set in appsettings.json");
    }

    var port = new Uri(url).Port;
    Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
        .MinimumLevel.Debug()
        .Enrich.WithProperty("AppPort", port)
        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
        .WriteTo.File(
            builder.Configuration["SerilogLogFile:Path"] ?? "log.txt",
            rollingInterval: RollingInterval.Day,
            outputTemplate:
            "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level}]: [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}"
        )
        .WriteTo.Seq(builder.Configuration["SeqServer:Url"] ?? "http://localhost:5341",LogEventLevel.Debug)
        .WriteTo.Email(new EmailSinkOptions()
        {
            From = "fieldlavender70@gmail.com",
            To = new List<string>() { "fieldlavender70@gmail.com" },
            Host = "smtp.sendgrid.net",
            Port = 587,
            Credentials = new NetworkCredential("apikey",
                credentials?.SendGridApiKey ?? string.Empty),
            ConnectionSecurity = SecureSocketOptions.None,
            Subject = new MessageTemplateTextFormatter(
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level}]: [{SourceContext}] [{EventId}] {Message}{NewLine}{Exception}"
            ),
        }, restrictedToMinimumLevel: LogEventLevel.Warning)
        .CreateLogger();

    builder.Services.AddApiVersioning(options =>
    {
        options.ReportApiVersions = true;
        options.DefaultApiVersion = new ApiVersion(1.0);
    }).AddMvc().AddApiExplorer(
        options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    builder.Services
        .AddScoped<IAnimalService,
            AnimalService>(); //Used scoped because a new connection context must be created on each request 
    builder.Services
        .AddScoped<IAnimalStatisticsService,
            AnimalStatisticsService>(); //Used scoped because a new connection context must be created on each request 
    builder.Services
        .AddScoped<IAuthService,
            AuthService>(); //Used scoped because a new connection context must be created on each request 
    builder.Services
        .AddScoped<IKeeperService,
            KeeperService>(); //Used scoped because a new connection context must be created on each request 
    builder.Services
        .AddScoped<IShiftService,
            ShiftService>(); //Used scoped because a new connection context must be created on each request 
    builder.Services
        .AddScoped<IUserRoleService,
            UserRoleService>(); //Used scoped because a new connection context must be created on each request 

    builder.Services
        .AddScoped<IUserService,
            UserService>(); //Used scoped because a new connection context must be created on each request 

    builder.Services.AddEndpointsApiExplorer();
    builder.Services
        .AddSingleton<IConfigureOptions<SwaggerGenOptions>,
            ConfigureSwaggerOptions>(); //Used singleton because it is one-time configuration
    builder.Services.AddSwaggerGen(options =>
    {
        options.OperationFilter<SwaggerDefaultValues>();
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description =
                "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token in the text input below. Example: 'Bearer 12345abcdef'",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header
                },
                new List<string>()
            }
        });

        var fileName = typeof(Program).Assembly.GetName().Name + ".xml";
        var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
        options.IncludeXmlComments(filePath);
    });
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            if (builder.Configuration["Jwt:Key"] == null) throw new Exception("Jwt:Key is not set in appsettings.json");

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = builder.Configuration["Jwt:Issuer"],
                ValidAudience = builder.Configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "secret"))
            };
        });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Admin", policy => policy.RequireRole(UserRole.UserRoleNames[UserRoleName.Admin]));
        options.AddPolicy("User", policy => policy.RequireRole(UserRole.UserRoleNames[UserRoleName.User]));
    });
    builder.Services.AddDbContext<DataContext>(options =>
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });
    builder.Services.AddHealthChecks()
        .AddCheck<DBHealthCheckService>(
            "DB check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "db" }
        )
        .AddCheck<UserHealthCheckService>(
            "User service check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "user" }
        )
        .AddCheck<AnimalHealthCheckService>(
            "Animal service check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "animal" }
        ).AddDbContextCheck<DataContext>()
        .AddCheck<KeeperHealthCheckService>(
            "Keeper service check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "keeper" }
        )
        .AddCheck<ShiftHealthCheckService>(
            "Shift service check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "shift" }
        ).AddCheck<UserRoleHealthCheckService>(
            "User role service check",
            failureStatus: HealthStatus.Unhealthy,
            tags: new[] { "user-role" }
        );
    builder.Services.AddHealthChecksUI().AddInMemoryStorage();
    builder.Host.UseSerilog();
    builder.Services.AddSingleton<IServerAddressesFeature, ServerAddressesFeature>();

    var app = builder.Build();

    app.UseSerilogRequestLogging(options =>
    {
        options.MessageTemplate =
            "{RemoteIpAddress} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        {
            diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
            diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
        };
    });
    app.MapHealthChecksUI();
    app.MapHealthChecks("/health-ui", new HealthCheckOptions
    {
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    app.MapHealthChecks("/health/db", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("db"),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthCheckResponse.WriteJsonResponse
    });
    app.MapHealthChecks("/health/user", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("user"),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthCheckResponse.WriteJsonResponse
    });
    app.MapHealthChecks("/health/animal", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("animal"),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthCheckResponse.WriteJsonResponse
    });
    app.MapHealthChecks("/health/keeper", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("keeper"),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthCheckResponse.WriteJsonResponse
    });
    app.MapHealthChecks("/health/shift", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("shift"),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthCheckResponse.WriteJsonResponse
    });
    app.MapHealthChecks("/health/user-role", new HealthCheckOptions
    {
        Predicate = check => check.Tags.Contains("user-role"),
        ResultStatusCodes =
        {
            [HealthStatus.Healthy] = StatusCodes.Status200OK,
            [HealthStatus.Degraded] = StatusCodes.Status200OK,
            [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
        },
        ResponseWriter = WriteHealthCheckResponse.WriteJsonResponse
    });

    app.UseExceptionHandling();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            foreach (var description in app.DescribeApiVersions())
            {
                options.SwaggerEndpoint(
                    $"/swagger/{description.GroupName}/swagger.json",
                    description.GroupName);
            }
        });
    }

    app.UseAuthorization();

    app.MapControllers();

    IHostApplicationLifetime hostApplicationLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
    hostApplicationLifetime.ApplicationStopping.Register(Log.CloseAndFlush);
    app.Run();
    
    
}
catch (Exception e)
{
    Log.Fatal("Server failed to start: {Message}", e.Message);
}
