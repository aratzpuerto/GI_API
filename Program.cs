using GI_API.Contracts;
using GI_API.Data;
using GI_API.Middlewares;
using GI_API.Models;
using GI_API.Services;
using Microsoft.EntityFrameworkCore;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Clear default logging providers
builder.Logging.ClearProviders();

// Use NLog as logging provider
builder.Host.UseNLog();

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskTypeContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

builder.Services.AddSingleton<ILoggerService, LoggerService>();

var app = builder.Build();

// Middleware pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseAuthorization();

// Global exception handling
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();
app.Run();