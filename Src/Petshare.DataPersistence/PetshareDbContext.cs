using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Petshare.Domain.Entities;

namespace Petshare.WebAPI.Data
{
    public class PetshareDbContext : DbContext
    {
        public PetshareDbContext(DbContextOptions<PetshareDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Adopter>().ToTable("Adopters");
            modelBuilder.Entity<Shelter>().ToTable("Shelters");

            modelBuilder.Entity<AdopterReport>();
            modelBuilder.Entity<AnnoucementReport>();
            modelBuilder.Entity<ShelterReport>();

            modelBuilder.Entity<AdopterAnnoucementFollowed>().ToTable("FollowedAnnoucements");
            modelBuilder.Entity<AdopterAnnoucementFollowed>()
                .HasKey(ad_an => new { ad_an.AdopterID, ad_an.AnnoucementID });
            modelBuilder.Entity<AdopterAnnoucementFinalised>().ToTable("FinalisedAnnoucements");
            modelBuilder.Entity<AdopterAnnoucementFinalised>()
                .HasKey(ad_an => new { ad_an.AdopterID, ad_an.AnnoucementID });

            var cascadeFKs = modelBuilder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);
            foreach (var fk in cascadeFKs)
                fk.DeleteBehavior = DeleteBehavior.Restrict;


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Pet> Pets { get; set; }
        public DbSet<Annoucement> Annoucements { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Report> Reports { get; set; }
    }
}
