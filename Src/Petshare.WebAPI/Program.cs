using Microsoft.EntityFrameworkCore;
using Petshare.DataPersistence;

namespace Petshare.WebAPI;

public class Program
{
    protected Program() { } 

    public static async Task Main(string[] args)
    {
        var webHost = CreateHostBuilder(args).Build();
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<PetshareDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });

        await webHost.RunAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
}