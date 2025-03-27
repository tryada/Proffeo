using Proffeo.Api;
using Proffeo.Api.Middleweares;
using Proffeo.Infrastructure;
using Proffeo.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args); 
{
    builder.Services
        .AddApi()
        .AddServicesDependencies()
        .AddInfrastructure(builder.Configuration);
    
    builder.Host.UseSerilog((context, configuration) =>
    {
        configuration.ReadFrom.Configuration(context.Configuration);
        if (builder.Environment.IsDevelopment())
            configuration.WriteTo.Console();
        else
            configuration.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
    });
}

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();