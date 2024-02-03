using Microsoft.OpenApi.Models;
using System.Reflection;
using LR6_WEB_NET.Services.AnimalService;
using LR6_WEB_NET.Services.KeeperService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IKeeperService, ZooKeeperService>(); //Used transient because it is a stateless service
builder.Services.AddTransient<IAnimalService, AnimalService>(); //Used transient because it is a stateless service
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Zoo API",
        Description = "An ASP.NET Core Web API for zoo management.",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


app.Run();
