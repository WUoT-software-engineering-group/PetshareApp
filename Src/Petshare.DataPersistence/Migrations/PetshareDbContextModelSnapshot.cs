﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Petshare.DataPersistence;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    [DbContext(typeof(PetshareDbContext))]
    partial class PetshareDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterAnnouncementFinalized", b =>
                {
                    b.Property<Guid>("AdopterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnnouncementID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdopterID", "AnnouncementID");

                    b.ToTable("FinalizedAnnouncements", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterAnnouncementFollowed", b =>
                {
                    b.Property<Guid>("AdopterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnnouncementID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdopterID", "AnnouncementID");

                    b.ToTable("FollowedAnnouncements", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Announcement", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("ClosingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastUpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("PetID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("AuthorID");

                    b.HasIndex("PetID");

                    b.ToTable("Announcements");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Application", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnnouncementID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfApplication")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsWithdrawn")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("AnnouncementID");

                    b.HasIndex("UserID");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Pet", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("datetime2");

                    b.Property<string>("Breed")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUri")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ShelterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Species")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.HasIndex("ShelterID");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Report", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Reports");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Report");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Petshare.Domain.Entities.ShelterAdopterVerification", b =>
                {
                    b.Property<Guid>("ShelterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdopterID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ShelterID", "AdopterID");

                    b.ToTable("VerifiedAdopters", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("Users");

                    b.UseTptMappingStrategy();
                });

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterReport", b =>
                {
                    b.HasBaseType("Petshare.Domain.Entities.Report");

                    b.Property<Guid>("AdopterID")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("AdopterReport");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.AnnouncementReport", b =>
                {
                    b.HasBaseType("Petshare.Domain.Entities.Report");

                    b.Property<Guid>("AnnouncementID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ShelterID")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("AnnouncementReport");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.ShelterReport", b =>
                {
                    b.HasBaseType("Petshare.Domain.Entities.Report");

                    b.Property<Guid>("ShelterID")
                        .HasColumnType("uniqueidentifier");

                    b.ToTable("Reports", t =>
                        {
                            t.Property("ShelterID")
                                .HasColumnName("ShelterReport_ShelterID");
                        });

                    b.HasDiscriminator().HasValue("ShelterReport");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Adopter", b =>
                {
                    b.HasBaseType("Petshare.Domain.Entities.User");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.ToTable("Adopters", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Shelter", b =>
                {
                    b.HasBaseType("Petshare.Domain.Entities.User");

                    b.Property<string>("FullShelterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAuthorized")
                        .HasColumnType("bit");

                    b.ToTable("Shelters", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Announcement", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.Shelter", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Petshare.Domain.Entities.Pet", "Pet")
                        .WithMany()
                        .HasForeignKey("PetID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Pet");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Application", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.Announcement", "Announcement")
                        .WithMany()
                        .HasForeignKey("AnnouncementID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Petshare.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Announcement");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Pet", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.Shelter", "Shelter")
                        .WithMany("Pets")
                        .HasForeignKey("ShelterID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Shelter");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.User", b =>
                {
                    b.OwnsOne("Petshare.Domain.Entities.Address", "Address", b1 =>
                        {
                            b1.Property<Guid>("UserID")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Country")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("PostalCode")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Province")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserID");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserID");
                        });

                    b.OwnsOne("Petshare.Domain.Entities.AnnouncementProvider", "AnnouncementProvider", b1 =>
                        {
                            b1.Property<Guid>("UserID")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("UserID");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserID");
                        });

                    b.Navigation("Address")
                        .IsRequired();

                    b.Navigation("AnnouncementProvider")
                        .IsRequired();
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Adopter", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("Petshare.Domain.Entities.Adopter", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Shelter", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.User", null)
                        .WithOne()
                        .HasForeignKey("Petshare.Domain.Entities.Shelter", "ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Shelter", b =>
                {
                    b.Navigation("Pets");
                });
#pragma warning restore 612, 618
        }
    }
}
