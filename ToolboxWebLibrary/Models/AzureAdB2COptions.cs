
namespace ToolboxWebLibrary.Models;

public class AzureAdB2COptions
{
#pragma warning disable CA2211 // Non-constant fields should not be visible
    public static AzureAdB2COptions Current;
#pragma warning restore CA2211 // Non-constant fields should not be visible

    public const string PolicyAuthenticationProperty = "Policy";

    public AzureAdB2COptions()
    {
        Current = this;
    }

    public string Path
    {
        get; set;
    }

    public string LoginUrl
    {
        get; set;
    }

    public string Tenant
    {
        get; set;
    }

    public string ClientId
    {
        get; set;
    }

    public string ClientSecret
    {
        get; set;
    }

    public string ExtensionAppId
    {
        get; set;
    } 

    public string ApiVersion
    {
        get; set;
    }
}
