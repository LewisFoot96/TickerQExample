using TickerQExample;
using Microsoft.EntityFrameworkCore;
using TickerQ.DependencyInjection;
using TickerQ.EntityFrameworkCore.DependencyInjection;
using TickerQ.Dashboard.DependencyInjection;

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

app.Run();
