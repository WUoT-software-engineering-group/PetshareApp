using Microsoft.EntityFrameworkCore;
using Petshare.Domain.Entities;

namespace Petshare.DataPersistence
{
    public interface IPetshareDbContext
    {
        DbSet<Announcement> Announcements { get; set; }
        DbSet<Application> Applications { get; set; }
        DbSet<Pet> Pets { get; set; }
        DbSet<Report> Reports { get; set; }
        DbSet<User> Users { get; set; }

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
