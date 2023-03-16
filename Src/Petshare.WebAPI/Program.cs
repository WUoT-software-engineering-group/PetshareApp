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
               .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}