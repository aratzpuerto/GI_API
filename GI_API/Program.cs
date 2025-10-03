using GI_API.Contracts;
using GI_API.Database;
using GI_API.Middlewares;
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

builder.Services.AddDbContext<GIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("GI_Connection")));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddScoped<TaskTypeService>();
builder.Services.AddScoped<DbService>();

var app = builder.Build();

// Run migrations and seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GIDbContext>();
    var dbService = scope.ServiceProvider.GetRequiredService<DbService>();

    // Seed only if empty
    await DbSeeder.Seed(db, dbService);
}

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