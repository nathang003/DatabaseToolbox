using System.Threading;

using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApi.Services;

public class MonitorLoop
{
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly ILogger<MonitorLoop> _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly IScrapeData _scrapeData;

    public MonitorLoop(IBackgroundTaskQueue taskQueue, ILogger<MonitorLoop> logger, IHostApplicationLifetime applicationLifetime, IScrapeData scrapeData)
    {
        _taskQueue = taskQueue;
        _logger = logger;
        _cancellationToken = applicationLifetime.ApplicationStopping;
        _scrapeData = scrapeData;
    }

    public void StartMonitorLoop()
    {
        _logger.LogInformation("MonitorAsync Loop is starting.");

        // Run a console user input loop in a background thread
        Task.Run(async () => await MonitorAsync());
    }

    private async ValueTask MonitorAsync()
    {
        while (!_cancellationToken.IsCancellationRequested)
        {

            try
            {
                await Task.Delay(TimeSpan.FromSeconds(300));
                // Need to rework
                // Should get a count of pending scrapes, then
                // if pending scrapes > 0 (or .Any()), then ScrapeWorkItem
                // else queue new suggested scrapes and trigger QueueNewScrapeWorkItem

                await _taskQueue.QueueBackgroundWorkItemAsync(ScrapeWorkItem);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }

    private async ValueTask ScrapeWorkItem(CancellationToken token)
    {
        Guid workerId = Guid.NewGuid();

        while (!token.IsCancellationRequested)
        {
            _logger.LogInformation($"Queueing Background Scrapes to ScrapeWorkerId: {workerId}.");

            try
            {
                int pendingScrapeCount = _scrapeData.GetUnassignedScrapeCount().Result;
                List<ScrapeDetailedModel> scrapes;
                if (pendingScrapeCount > 0)
                {
                    scrapes = _scrapeData.GetAllUnassignedScrapes().Result.Take(5).ToList();
                }
                else
                {
                    scrapes = await ConvertSuggestionsToScrapes(_scrapeData.GetTopSuggestedScrapes().Result.Take(5).ToList());
                }

                if (scrapes.Any())
                {
                    foreach (ScrapeDetailedModel scrape in scrapes)
                    {
                        // Add scrape to dbo.scrape table.
                        await _scrapeData.AddScrape(scrape.ScrapeScope, scrape.DynamicScrapeObject, scrape.DynamicScrapeObjectId, scrape.ScrapeScheduledDate);

                        // Assign worker to scrape
                        await UpdateScrape(scrape, ScrapeStatus.Assigned, workerId);

                        // Begin scrape execution
                        await UpdateScrape(scrape, ScrapeStatus.Executing, workerId);
                    }

                    foreach (ScrapeDetailedModel scrape in scrapes)
                    {
                        // Execute scrape

                        try
                        {
                            if (await ExecuteScrape(scrape))
                            {
                                await UpdateScrape(scrape, ScrapeStatus.Completed, workerId);
                            }
                            else
                            {
                                await UpdateScrape(scrape, ScrapeStatus.Failed, workerId);
                            }
                        }
                        catch
                        {
                            await UpdateScrape(scrape, ScrapeStatus.Failed, workerId);
                        }
                    }
                }
                else
                {
                    _logger.LogInformation($"No Pending Background Scrape found to assign to ScrapeWorkerId: {workerId}.");
                }
            }
            catch (OperationCanceledException)
            {
                // Prevent throwing if the Delay is cancelled
            }

            _logger.LogInformation($"Finished Background Scrapes for ScrapeWorkerId: {workerId}.");
        }
    }

    public async Task UpdateScrape(ScrapeDetailedModel scrape, ScrapeStatus status, Guid workerId)
    {
        if (status.ToString() == "Assigned")
        {
            scrape.ScrapeWorkerId = workerId;
        }

        scrape.ScrapeStatusId = (int)status;
        scrape.ScrapeStatusName = status.ToString();

        _logger.LogInformation($"Background Scrape for {scrape.DynamicScrapeObjectName} assigned to ScrapeWorkerId ({workerId}) is now {status.ToString()}.");

        await _scrapeData.UpdateScrape(scrape);
    }

    public enum ScrapeStatus : int
    {
        Scheduled = 2,
        Assigned = 3,
        Executing = 4,
        Completed = 5,
        Failed = 6
    }

    public async Task<bool> ExecuteScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeQueue.ExecuteScrape(ScrapeDetailedModel scrape)");
        switch (scrape.ScrapeScope)
        {
            case "Server":
                return await _scrapeData.ExecuteServerScrape(scrape);
            case "Database":
                return await _scrapeData.ExecuteDatabaseScrape(scrape);
            case "Schema":
                return await _scrapeData.ExecuteSchemaScrape(scrape);
            case "Table":
                return await _scrapeData.ExecuteTableScrape(scrape);
            case "Field":
                return await _scrapeData.ExecuteFieldScrape(scrape);
            case "Index":
                return await _scrapeData.ExecuteIndexScrape(scrape);
            case "Constraint":
                return await _scrapeData.ExecuteConstraintScrape(scrape);
            case "Foreign Key":
                return await _scrapeData.ExecuteForeignKeyScrape(scrape);
            default:
                //throw new NotImplementedException();
                return false;
        }
    }

    public async Task<List<ScrapeDetailedModel>> ConvertSuggestionsToScrapes(List<ScrapeSuggestionModel> suggestions)
    {
        if(suggestions != null && suggestions.Any())
        {
            List<ScrapeDetailedModel> convertedScrapes = new List<ScrapeDetailedModel>();

            foreach(ScrapeSuggestionModel suggestion in suggestions)
            {
                ScrapeDetailedModel convertedScrape = new ScrapeDetailedModel(suggestion);
                convertedScrapes.Add(convertedScrape);
            }

            return convertedScrapes;
        }
        else
        {
            return new List<ScrapeDetailedModel>();
        }
    }
}
