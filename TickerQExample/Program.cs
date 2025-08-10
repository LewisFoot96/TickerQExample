using Microsoft.EntityFrameworkCore;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Utilities;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces.Managers;
using TickerQ.Utilities.Models.Ticker;
using TickerQExample;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql("Host=localhost;Port=5432;Database=scheduling;Username=postgres;Password=Rockyts3905ts!"));

builder.Services.AddTickerQ(options =>
{
    options.AddOperationalStore<MyDbContext>(efCoreOpt =>
    {
        efCoreOpt.UseModelCustomizerForMigrations();
        efCoreOpt.CancelMissedTickersOnApplicationRestart();
    });

    options.AddDashboard("/ticker");
   // options.AddDashboardBasicAuth();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseTickerQ();

app.MapGet("/send-welcome", async (ITimeTickerManager<TimeTicker> timeTickerManager, CancellationToken ct) =>
{
    await timeTickerManager.AddAsync(new TimeTicker
    {
        Function = nameof(MyJobs.SendWelcome),
        ExecutionTime = DateTime.UtcNow.AddSeconds(1),
        Request = TickerHelper.CreateTickerRequest<string>("User123"),
        Retries = 3,
        RetryIntervals = [30, 60, 120], // Retry after 30s, 60s, then 2min

        // Optional batching
        //BatchParent = Guid.Parse("...."),
        //BatchRunCondition = BatchRunCondition.OnSuccess
    });
    return Results.Ok();
});

app.Run();
