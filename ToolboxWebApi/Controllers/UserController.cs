using System.IdentityModel.Tokens;

using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace ToolboxWebApi.Controllers;

//[Authorize]
public class UserController : Controller
{
    public UserController(ILogger<UserController> logger)
    {
        _logger = logger;
    }

    private readonly ILogger<UserController> _logger;
    public const string SessionKeyUserListSkipToken = "_UserListSkipToken";


    public static string UserListSkipToken
    {
        get; set;
    }


    public IActionResult Users()
    {
        return View();
    }
}
