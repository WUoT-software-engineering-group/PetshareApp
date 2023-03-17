using Microsoft.EntityFrameworkCore;
using Petshare.Domain.Entities;

namespace Petshare.DataPersistence;

public class RepositoryDbContext : DbContext
{
    public DbSet<Shelter>? Shelter { get; set; }
}