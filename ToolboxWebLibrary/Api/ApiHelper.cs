using System.Configuration;
using System.Net.Http.Headers;

using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using Microsoft.Web.Administration;

namespace ToolboxWebLibrary.Api;

public class ApiHelper : IApiHelper
{
    private readonly ITokenAcquisition tokenAcquisition;

    private HttpClient _apiClient
    {
        get; set;
    }

    public ApiHelper(ITokenAcquisition tokenAcquisition)
    {
        this.tokenAcquisition = tokenAcquisition;
        InitializeClient();
    }

    private IConfiguration _config;

    private async void InitializeClient()
    {
        //string api = _config.GetValue<string>("ToolboxApi");
        //var settings = System.Configuration.ConfigurationManager.AppSettings["chicken"];
        //string api = settings.ToString();
        string api = string.Empty;
        _apiClient = new();

        if (string.IsNullOrWhiteSpace(api))
        {
            _apiClient.BaseAddress = new Uri("https://localhost:7087/");
        }
        else
        {
            _apiClient.BaseAddress = new Uri(api);
        }

        _apiClient.Timeout = TimeSpan.FromSeconds(1000);
        _apiClient.DefaultRequestHeaders.Accept.Clear();
        _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //string[] scopes = new string[] { "api://2c475d36-06dc-400f-8404-f12c148c53d5/data.readonly" };
        string[] scopes = new string[] { "api://2c475d36-06dc-400f-8404-f12c148c53d5/App.ReadWrite" };
        string token = String.Empty;

        try
        {
            token = await tokenAcquisition.GetAccessTokenForUserAsync(scopes);
        }
        catch (MicrosoftIdentityWebChallengeUserException ex)
        {
            tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeader(scopes, ex.MsalUiRequiredException);
        }
        catch (MsalUiRequiredException ex)
        {
            tokenAcquisition.ReplyForbiddenWithWwwAuthenticateHeader(scopes, ex);
        }

        _apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public HttpClient ApiClient
    {
        get
        {
            return _apiClient;
        }
    }
}
