
using Microsoft.Extensions.Hosting;

namespace ToolboxWebApi.Services;

public class ScrapeWorkerService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private readonly ILogger<ScrapeWorkerService> _logger;
    private Timer _timer;

    public ScrapeWorkerService(ILogger<ScrapeWorkerService> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug("Starting Scrape Worker Service.");

        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(300));

        return Task.CompletedTask;
    }

    private void DoWork(object state)
    {
        var count = Interlocked.Increment(ref executionCount);

        _logger.LogInformation($"Scrape Worker Service is working. Count: {count}");
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Scrape Worker Service is stopping.");

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}
