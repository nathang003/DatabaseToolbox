

using System.Net.Http;
using System.Text;

using Newtonsoft.Json;

namespace ToolboxWebLibrary.Api;

public class ScrapeEndpoint : IScrapeEndpoint
{
    private readonly IApiHelper _apiHelper;

    public ScrapeEndpoint(IApiHelper apiHelper)
    {
        _apiHelper = apiHelper;
    }

    public async Task<List<ScrapeModel>> GetAllScrapes()
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetAllScrapes()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Scrape/GetAllScrapes"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeModel> result = await response.Content.ReadAsAsync<List<ScrapeModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ScrapeDetailedModel>> GetAllScrapesDetailed()
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetAllScrapesDetailed()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Scrape/GetAllScrapesDetailed"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeDetailedModel> result = await response.Content.ReadAsAsync<List<ScrapeDetailedModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ScrapeModel>> GetAllUnassignedScrapes()
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetAllUnassignedScrapes()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Scrape/GetAllUnassignedScrapes"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeModel> result = await response.Content.ReadAsAsync<List<ScrapeModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ScrapeSuggestionModel>> GetSuggestedScrapes()
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetSuggestedScrapes()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Scrape/GetSuggestedScrapes"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeSuggestionModel> result = await response.Content.ReadAsAsync<List<ScrapeSuggestionModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ScrapeSuggestionModel>> GetTopSuggestedScrapes()
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetTopSuggestedScrapes()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Scrape/GetTopSuggestedScrapes"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeSuggestionModel> result = await response.Content.ReadAsAsync<List<ScrapeSuggestionModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<int> GetSuggestionCount()
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetSuggestionCount()");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync("/api/Scrape/GetSuggestionCount"))
        {
            if (response.IsSuccessStatusCode)
            {
                int result = await response.Content.ReadAsAsync<int>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ScrapeModel>> GetScrapeById(Guid scrapeId)
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetScrapeById(Guid scrapeId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Scrape/GetScrapeById/{scrapeId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeModel> result = await response.Content.ReadAsAsync<List<ScrapeModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task<List<ScrapeDetailedModel>> GetMyAssignedScrapes(string userId)
    {
        Console.WriteLine("Entering ScrapeEndpoint.GetMyAssignedScrapes(string userId)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.GetAsync($"/api/Scrape/GetMyAssignedScrapes/{userId}"))
        {
            if (response.IsSuccessStatusCode)
            {
                List<ScrapeDetailedModel> result = await response.Content.ReadAsAsync<List<ScrapeDetailedModel>>();
                return result;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task AddScrape(string scrapeScope, string dynamicScrapeObject, int dynamicScrapeObjectId, DateTime scrapeScheduledDateTime)
    {
        Console.WriteLine("Entering ScrapeEndpoint.AddScrape(string scrapeScope, string dynamicScrapeObject, int dynamicScrapeObjectId, DateTime scrapeScheduledDateTime)");

        ScrapeModel scrape = new()
        {
            ScrapeScope = scrapeScope,
            DynamicScrapeObject = dynamicScrapeObject,
            DynamicScrapeObjectId = dynamicScrapeObjectId,
            ScrapeScheduledDate = scrapeScheduledDateTime
        };

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeModel>($"/api/Scrape/AddScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
    }

    public async Task UpdateScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.UpdateScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PutAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/UpdateScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.ReasonPhrase);
            }  
        }
    }

    public async Task<bool> ExecuteServerScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteServerScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteServerScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }

    public async Task<bool> ExecuteDatabaseScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteDatabaseScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteDatabaseScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }

    public async Task<bool> ExecuteSchemaScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteSchemaScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteSchemaScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }

    public async Task<bool> ExecuteTableScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteTableScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteTableScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }

    public async Task<bool> ExecuteFieldScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteFieldScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteFieldScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }

    public async Task<bool> ExecuteIndexScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteIndexScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteIndexScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }

    public async Task<bool> ExecuteConstraintScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteConstraintScrape(ScrapeDetailedModel scrape)");

        try
        {
            using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteConstraintScrape", scrape))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.ReasonPhrase);
                    //throw new Exception(response.ReasonPhrase);
                    return false;
                }

                return true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ScrapeEndpoint.ExecuteConstraintScrape experienced an exception: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ExecuteForeignKeyScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeEndpoint.ExecuteForeignKeyScrape(ScrapeDetailedModel scrape)");

        using (HttpResponseMessage response = await _apiHelper.ApiClient.PostAsJsonAsync<ScrapeDetailedModel>($"/api/Scrape/ExecuteForeignKeyScrape", scrape))
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.ReasonPhrase);
                //throw new Exception(response.ReasonPhrase);
                return false;
            }

            return true;
        }
    }
}
