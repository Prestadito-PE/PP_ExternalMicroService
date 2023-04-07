using Microsoft.OpenApi.Models;
using Prestadito.External.API.Endpoints;
using Prestadito.External.Application.Manager.Extensions;
using Prestadito.External.Application.Manager.Utilities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSExternalControllers();
builder.Services.AddValidators();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Prestadito External Microservice",
        Description = "ASP.NET Core Web API Control Schedule System",
        TermsOfService = new Uri("https://prestadito.pe/terms"),
        Contact = new OpenApiContact
        {
            Name = "Prestadito.pe",
            Email = "contacto@prestadito.pe",
            Url = new Uri("https://prestadito.pe"),
        },
        License = new OpenApiLicense
        {
            Name = "Use under LICX",
            Url = new Uri("https://prestadito.pe"),
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("v1/swagger.json", "Prestadito.Micro.External.API");
    });
    app.Services.GetRequiredService<ILoggerFactory>().AddSyslog(app.Configuration.GetValue<string>("Papertrail:host"), app.Configuration.GetValue<int>("Papertrail:port"));
}

app.UseExternalEndpoints();

app.Run();
