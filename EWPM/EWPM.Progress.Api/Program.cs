using EWPM.Progress.Api.Interface;
using EWPM.Progress.Api.Middleware;
using EWPM.Progress.Api.Services;
using EWPM.Repository.Progress.Data;
using EWPM.Repository.Progress.Repository;
using EWPM.Repository.Progress.Interface;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProgressDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString")));
builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Redis server
    options.InstanceName = "EWPM_";
});
builder.Services.AddSingleton<IProgressQueue, ProgressQueue>();
builder.Services.AddHostedService<Worker>();
builder.Services.AddHostedService<ProgressWorker>();
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.MapControllers();
app.Run();

