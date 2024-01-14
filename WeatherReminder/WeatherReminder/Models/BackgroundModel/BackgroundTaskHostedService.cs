using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WeatherReminder.Models.BackgroundModel;

public class BackgroundTaskHostedService : IHostedService, IDisposable
{
    private Timer _timer;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger<BackgroundTaskHostedService> _logger;

    public BackgroundTaskHostedService(IServiceScopeFactory serviceScopeFactory, ILogger<BackgroundTaskHostedService> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background task started.");

        _timer = new Timer(_ =>
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var backgroundTasks = scope.ServiceProvider.GetRequiredService<IBackgroundTasks>();
                backgroundTasks.SendUsersReminders();
                _logger.LogInformation("Sent users reminders.");
            }
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));

        // Schedule UpdateUsersWeatherForecast to run at midnight (0:00)
        DateTime nextMidnight = DateTime.Today.AddDays(1);
        TimeSpan timeUntilMidnight = nextMidnight - DateTime.Now;
        _timer = new Timer(_ =>
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var backgroundTasks = scope.ServiceProvider.GetRequiredService<IBackgroundTasks>();
                backgroundTasks.UpdateUsersWeatherForecast();
                _logger.LogInformation("Updated users weatherforecast.");
            }
        }, null, timeUntilMidnight, TimeSpan.FromHours(24));

        // Schedule DeleteUnconfirmedAccounts to run every 15 minutes
        _timer = new Timer(_ =>
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var backgroundTasks = scope.ServiceProvider.GetRequiredService<IBackgroundTasks>();
                backgroundTasks.DeleteUnconfirmedAccounts();
                _logger.LogInformation("Deleted unconfirmed accounts.");
            }
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        _logger.LogInformation("Background task stopped.");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
