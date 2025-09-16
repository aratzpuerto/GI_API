using GI_API.Contracts;
using GI_API.Data;
using GI_API.Middlewares;
using GI_API.Models;
using GI_API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskTypeContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

builder.Services.AddSingleton<ILoggerService, LoggerService>();
builder.Services.AddTransient<GlobalExceptionHandlingMiddleware>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();
//app.MapGet("/taskTypes", async(TaskTypeContext db) => await db.TaskTypes.ToListAsync());

//app.MapPost("/taskTypes", async (TaskType taskType, TaskTypeContext db) =>
//{
//    db.TaskTypes.Add(taskType);
//    await db.SaveChangesAsync();

//    return Results.Ok();
//}
//);
app.MapControllers();

app.Run();
