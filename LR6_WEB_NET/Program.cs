using System.Text;
using Asp.Versioning;
using LR6_WEB_NET.Data.DatabaseContext;
using LR6_WEB_NET.Extensions;
using LR6_WEB_NET.Models.Database;
using LR6_WEB_NET.Models.Enums;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.AnimalStatisticsService;
using LR6_WEB_NET.Services.AuthService;
using LR6_WEB_NET.Services.KeeperService;
using LR6_WEB_NET.Services.ShiftService;
using LR6_WEB_NET.Services.UserRoleService;
using LR6_WEB_NET.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
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
builder.Services.AddScoped<IAnimalService, AnimalService>(); //Used scoped because a new connection context must be created on each request 
builder.Services
    .AddScoped<IAnimalStatisticsService,
        AnimalStatisticsService>(); //Used scoped because a new connection context must be created on each request 
builder.Services.AddScoped<IAuthService, AuthService>(); //Used scoped because a new connection context must be created on each request 
builder.Services.AddScoped<IKeeperService, KeeperService>(); //Used scoped because a new connection context must be created on each request 
builder.Services.AddScoped<IShiftService, ShiftService>(); //Used scoped because a new connection context must be created on each request 
builder.Services.AddScoped<IUserRoleService, UserRoleService>(); //Used scoped because a new connection context must be created on each request 

builder.Services.AddScoped<IUserService, UserService>(); //Used scoped because a new connection context must be created on each request 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();//Used singleton because it is one-time configuration
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
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.AddPolicy("Admin", policy => policy.RequireRole(UserRole.UserRoleNames[UserRoleName.Admin]));
    options.AddPolicy("User", policy => policy.RequireRole(UserRole.UserRoleNames[UserRoleName.User]));
});
builder.Services.AddDbContext<DataContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapHealthChecks("/healthz");
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


app.Run();