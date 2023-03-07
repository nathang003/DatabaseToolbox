
using Azure.Identity;

using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ToolboxWebApi.Services;

var builder = WebApplication.CreateBuilder(args);


builder.ConfigureServices();

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("VaultUri"));
builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"))
    .EnableTokenAcquisitionToCallDownstreamApi()
    //.AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
    .AddInMemoryTokenCaches();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//app.Run();
await app.StartAsync();

var monitorLoop = app.Services.GetRequiredService<MonitorLoop>();
monitorLoop.StartMonitorLoop();
Console.WriteLine("Launched monitor loop.");

await app.WaitForShutdownAsync();