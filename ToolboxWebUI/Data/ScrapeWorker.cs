

namespace ToolboxWebUI.Data;

public class ScrapeWorker
{
    private readonly ILogger<ScrapeWorker> logger;
    private List<ScrapeDetailedModel> scrapes;

    public ScrapeWorker(ILogger<ScrapeWorker> logger)
    {    
        this.logger = logger;
    }

    public async Task RunScrapes(CancellationToken cancellationToken)
    {
        Guid workerGuid = Guid.NewGuid();

        while (!cancellationToken.IsCancellationRequested)
        {
            //scrapes = IScrapeEndpoint.

            if (scrapes is not null && scrapes.Count > 0)
            {

                ScrapeDetailedModel scrape = scrapes.FirstOrDefault();
                logger.LogInformation($"Worker {workerGuid} running scrape {scrape.ScrapeId}.");
            }

            Thread.Sleep(5000);
        }
    }
}
