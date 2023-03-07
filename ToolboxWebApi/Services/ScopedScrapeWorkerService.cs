namespace ToolboxWebApi.Services;

internal class ScopedScrapeWorkerService : IScopedScrapeWorkerService
{
    private int executionCount = 0;
    private readonly ILogger _logger;

    public ScopedScrapeWorkerService(ILogger<ScopedScrapeWorkerService> logger)
    {
        _logger = logger;
    }

    public async Task DoWork(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            executionCount++;

            _logger.LogInformation($"Scoped Scrape Worker Service is working. Count: {executionCount}");

            await Task.Delay(300000, stoppingToken);
        }
    }
}
