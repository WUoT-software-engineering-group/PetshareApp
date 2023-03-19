using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Petshare.DataPersistence;

public class PetshareDbContextFactory : IDesignTimeDbContextFactory<PetshareDbContext>
{
    public PetshareDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var builder = new DbContextOptionsBuilder<PetshareDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        builder.UseSqlServer(connectionString, b => b.MigrationsAssembly("Petshare.DataPersistence"));


        return new PetshareDbContext(builder.Options);
    }
}
