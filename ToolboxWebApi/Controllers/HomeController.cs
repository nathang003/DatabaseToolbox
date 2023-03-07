
using Azure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Microsoft.Identity.Web;
using System.Diagnostics;

namespace ToolboxWebApi.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly GraphServiceClient _graphServiceClient;
    private readonly MicrosoftIdentityConsentAndConditionalAccessHandler _consentHandler;
    private string[] _graphScopes;


    public HomeController(ILogger<HomeController> logger,
                        IConfiguration configuration,
                        GraphServiceClient graphServiceClient,
                        MicrosoftIdentityConsentAndConditionalAccessHandler consentHandler)
    {
        _logger = logger;
        _graphServiceClient = graphServiceClient;
        _consentHandler = consentHandler;

        // Capture the Scopes for Graph that were used in the original request for an Access token (AT) for MS Graph as
        // they'd be needed again when requesting a fresh AT for Graph during claims challenge processing
        _graphScopes = configuration.GetValue<string>("DownstreamApi:Scopes")?.Split(' ');
    }


    //[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public IActionResult Index()
    {
        return View();
    }


    //[AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    public async Task<IActionResult> Profile()
    {
        User? currentUser = null;

        try
        {
            currentUser = await _graphServiceClient.Me.Request().GetAsync();
        }
        // Catch CAE exception from Graph SDK
        catch (ServiceException svcex) when (svcex.Message.Contains("Continuous access evaluation resulted in claims challenge"))
        {
            try
            {
                Console.WriteLine($"{svcex}");
                string claimChallenge = WwwAuthenticateParameters.GetClaimChallengeFromResponseHeaders(svcex.ResponseHeaders);
                _consentHandler.ChallengeUser(_graphScopes, claimChallenge);
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                _consentHandler.HandleException(ex);
            }
        }

        try
        {
            // Get user photo
            using (var photoStream = await _graphServiceClient.Me.Photo.Content.Request().GetAsync())
            {
                byte[] photoByte = ((MemoryStream)photoStream).ToArray();
                ViewData["Photo"] = Convert.ToBase64String(photoByte);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            ViewData["Photo"] = null;
        }

        ViewData["Me"] = currentUser;
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }


    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    [Route("api/GetSecret")]
    [HttpGet(Name="GetSecret")]
    private string GetSecretFromKeyVault()
    {
        string uri = Environment.GetEnvironmentVariable("VaultUri");
        SecretClient client = new SecretClient(new Uri(uri), new DefaultAzureCredential());

        Response<KeyVaultSecret> secret = client.GetSecretAsync("Graph-App-Secret").Result;

        return secret.Value.Value;
    }
}
