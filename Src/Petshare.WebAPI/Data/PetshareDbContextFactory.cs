using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Petshare.WebAPI.Data;

public class PetshareDbContextFactory : IDesignTimeDbContextFactory<PetshareDbContext>
{
    public PetshareDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<PetshareDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Petshare.WebAPI"));


        return new PetshareDbContext(builder.Options);
    }
}
