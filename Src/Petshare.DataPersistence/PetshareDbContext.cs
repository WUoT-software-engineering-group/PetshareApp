using Microsoft.EntityFrameworkCore;
using Petshare.Domain.Entities;

namespace Petshare.DataPersistence
{
    public class PetshareDbContext : DbContext, IPetshareDbContext
    {
        public PetshareDbContext(DbContextOptions<PetshareDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adopter>().ToTable("Adopters");
            modelBuilder.Entity<Shelter>().ToTable("Shelters");

            modelBuilder.Entity<AdopterReport>();
            modelBuilder.Entity<AnnouncementReport>();
            modelBuilder.Entity<ShelterReport>();

            modelBuilder.Entity<AdopterAnnouncementFollowed>().ToTable("FollowedAnnouncements");
            modelBuilder.Entity<AdopterAnnouncementFollowed>()
                .HasKey(adAn => new { adAn.AdopterID, adAn.AnnouncementID });
            modelBuilder.Entity<AdopterAnnouncementFinalized>().ToTable("FinalizedAnnouncements");
            modelBuilder.Entity<AdopterAnnouncementFinalized>()
                .HasKey(adAn => new { adAn.AdopterID, adAn.AnnouncementID });

            modelBuilder.Entity<ShelterAdopterVerification>().ToTable("VerifiedAdopters");
            modelBuilder.Entity<ShelterAdopterVerification>()
                .HasKey(shAd => new { shAd.ShelterID, shAd.AdopterID });
            
            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => fk is { IsOwnership: false, DeleteBehavior: DeleteBehavior.Cascade });
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Pet> Pets { get; set; }

        public DbSet<Announcement> Announcements { get; set; }

        public DbSet<Application> Applications { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Report> Reports { get; set; }
    }
}
