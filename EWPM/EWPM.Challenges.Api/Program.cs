using EWPM.Progress.Api.Middleware;
using EWPM.Repository.Challenges.Data;
using EWPM.Repository.Challenges.Interface;
using EWPM.Repository.Challenges.Repository;
using EWPM.Shared.Helper;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ChallengesDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,               
                maxRetryDelay: TimeSpan.FromSeconds(10),
                errorNumbersToAdd: null      
            );
        }));
builder.Services.AddControllers();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Redis server
    options.InstanceName = "EWPM_";
});
builder.Services.AddHttpClient(Constants.Progress, client =>
{
    client.BaseAddress = new Uri("https://localhost:7111/"); 
});
builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("localhost:6379"));
builder.Services.AddScoped<IChallengesRepository, ChallengesRepository>();
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

