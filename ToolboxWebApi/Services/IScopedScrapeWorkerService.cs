namespace ToolboxWebApi.Services
{
    internal interface IScopedScrapeWorkerService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}