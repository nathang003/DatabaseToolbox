using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

using ToolboxWebLibrary.Internal.DataAccess;

using Blazored.Toast;

namespace ToolboxWebUI;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');
        
        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.Lax;
        });

        // Add services to the container.
        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
            .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();
        builder.Services.AddControllersWithViews()
            .AddMicrosoftIdentityUI();

        builder.Services.Configure<OpenIdConnectOptions>(AzureADDefaults.OpenIdScheme,
            opt =>
            {
                var resourceUri = new Uri(builder.Configuration["ToolboxApi"]);
                var resource = $"{resourceUri.Scheme}://{resourceUri.Host}/";
                opt.ResponseType = "code";
                opt.SaveTokens = true;
                opt.Scope.Add("User.Read");
                opt.Scope.Add("User.ReadBasic.All");
                opt.Scope.Add("profile");
                opt.Scope.Add("openid");
                opt.Scope.Add(new Uri(builder.Configuration["ToolboxApi"]).Host);
                opt.Resource = resource;
            });
        builder.Services.AddScoped<TokenProvider>();
        builder.Services.AddHttpClient("API", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration["ToolboxApi"]);
        });

        builder.Services.AddAuthorization(options =>
        {
            // By default, all incoming requests will be authorized according to the default policy
            options.FallbackPolicy = options.DefaultPolicy;
            //options.AddPolicy("DataTeam", policyBuilder => policyBuilder.RequireClaim("groups", ""));
        });

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor()
            .AddMicrosoftIdentityConsentHandler();

        // Add singletons
        builder.Services.AddTransient<IApiHelper, ApiHelper>();
        builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddScoped<IServerEndpoint, ServerEndpoint>();
        builder.Services.AddScoped<IDatabaseEndpoint, DatabaseEndpoint>();
        builder.Services.AddScoped<ISchemaEndpoint, SchemaEndpoint>();
        builder.Services.AddScoped<ITableEndpoint, TableEndpoint>();
        builder.Services.AddScoped<IFieldEndpoint, FieldEndpoint>();
        builder.Services.AddScoped<IScrapeEndpoint, ScrapeEndpoint>();
        builder.Services.AddScoped<IForeignKeyEndpoint, ForeignKeyEndpoint>();
        builder.Services.AddScoped<IIndexEndpoint, IndexEndpoint>();
        builder.Services.AddScoped<IConstraintEndpoint, ConstraintEndpoint>();
        builder.Services.AddScoped<IDatabaseObjectEndpoint, DatabaseObjectEndpoint>();
        builder.Services.AddScoped<IDataTableLineageEndpoint, DataTableLineageEndpoint>();
        builder.Services.AddSingleton<IDisplayHelper, DisplayHelper>();
        builder.Services.AddBlazoredToast();
    }
}
