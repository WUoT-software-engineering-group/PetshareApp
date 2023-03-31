using Azure.Identity;
using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace Petshare.WebAPI;

public class Program
{
    protected Program() { } 

    public static async Task Main(string[] args)
    {
        var webHost = CreateHostBuilder(args).Build();

        await webHost.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
               .ConfigureAppConfiguration((context, config) =>
               {
                   var builtConfig = config.Build();

                   var client = new KeyVaultClient((authority, resource, scope) =>
                   {                       
                       var credential = new DefaultAzureCredential(false);
                       return Task.FromResult(credential.GetToken(
                           new Azure.Core.TokenRequestContext(
                               new[] {"https://vault.azure.net/.default"})).Token);
                   });

                   config.AddAzureKeyVault(
                       builtConfig["VaultUri"],
                       client, 
                       new DefaultKeyVaultSecretManager());
               });
}