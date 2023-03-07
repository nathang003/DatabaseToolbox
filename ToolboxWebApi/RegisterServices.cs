using Microsoft.OpenApi.Models;

using ToolboxWebApi.Services;
using ToolboxWebApiLibrary.Internal.DataAccess;
using ToolboxWebApiLibrary.Helpers;

namespace ToolboxWebApi;

public static class RegisterServices
{
    public static void ConfigureServices(this WebApplicationBuilder builder)
    {
        //string resource = "https://localhost:7087.com";
        //var clientId = "2c475d36-06dc-400f-8404-f12c148c53d5";
        //var tenantId = "53c87e3d-377c-407d-a9d1-0cdb69c12968";
        //var redirectUri = "/signin-oidc";

        ////var authBuilder = PublicClientApplicationBuilder.Create(clientId)
        ////                    .WithAuthority(AadAuthorityAudience.AzureAdMyOrg)
        ////                    .WithTenantId(tenantId)
        ////                    .WithRedirectUri(resource + redirectUri)       
        ////                    .Build();
        //var scope = resource + "/.default";
        //string[] scopes = { scope };

        //AuthenticationResult token = authBuilder.AcquireTokenInteractive(scopes).ExecuteAsync().Result;

        // Add services to the container.
        //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        //    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below. \r\n\r\nExample: \"bearer {token}\""
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });


        builder.Services.AddTransient<ISqlDataAccess, SqlDataAccess>();
        builder.Services.AddSingleton<IServerData, ServerData>();
        builder.Services.AddSingleton<IDatabaseData, DatabaseData>();
        builder.Services.AddSingleton<ISchemaData, SchemaData>();
        builder.Services.AddSingleton<ITableData, TableData>();
        builder.Services.AddSingleton<IFieldData, FieldData>();
        builder.Services.AddSingleton<IForeignKeyData, ForeignKeyData>();
        builder.Services.AddSingleton<IIndexData, IndexData>();
        builder.Services.AddSingleton<IConstraintData, ConstraintData>();
        builder.Services.AddSingleton<IScrapeData, ScrapeData>();
        builder.Services.AddSingleton<IDatabaseObjectData, DatabaseObjectData>();
        builder.Services.AddSingleton<IDataTableLineageData, DataTableLineageData>();
        builder.Services.AddSingleton<IConverterHelper, ConverterHelper>();

        // Register scoped service
        builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
        builder.Services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

        builder.Services.AddScoped<ScrapeWorkerService>();
        
        builder.Services.AddSingleton<MonitorLoop>();
        builder.Services.AddHostedService<QueuedHostedService>();
        builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx =>
        {
            if (!int.TryParse(builder.Configuration["QueueCapacity"], out int queueCapacity))
            {
                queueCapacity = 100;
            }

            return new BackgroundTaskQueue(queueCapacity);
        });
    }
}
