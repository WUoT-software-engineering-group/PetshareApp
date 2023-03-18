﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Petshare.WebAPI.Data;

#nullable disable

namespace Petshare.WebAPI.Migrations
{
    [DbContext(typeof(PetshareDbContext))]
    partial class PetshareDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterAnnoucementFinalised", b =>
                {
                    b.Property<Guid>("AdopterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnnoucementID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdopterID", "AnnoucementID");

                    b.HasIndex("AnnoucementID");

                    b.ToTable("FinalisedAnnoucements", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterAnnoucementFollowed", b =>
                {
                    b.Property<Guid>("AdopterID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnnoucementID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AdopterID", "AnnoucementID");

                    b.HasIndex("AnnoucementID");

                    b.ToTable("FollowedAnnoucements", (string)null);
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Annoucement", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AuthorID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ClosingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

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

                    b.ToTable("Annoucements");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Application", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AnnoucementID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfApplication")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsWithdrawed")
                        .HasColumnType("bit");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.HasIndex("AnnoucementID");

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

                    b.Property<byte[]>("Photo")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

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

            modelBuilder.Entity("Petshare.Domain.Entities.AnnoucementReport", b =>
                {
                    b.HasBaseType("Petshare.Domain.Entities.Report");

                    b.Property<Guid>("AnnoucementID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ShelterID")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("AnnoucementReport");
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

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterAnnoucementFinalised", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.Adopter", "Adopter")
                        .WithMany()
                        .HasForeignKey("AdopterID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Petshare.Domain.Entities.Annoucement", "annoucement")
                        .WithMany()
                        .HasForeignKey("AnnoucementID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Adopter");

                    b.Navigation("annoucement");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.AdopterAnnoucementFollowed", b =>
                {
                    b.HasOne("Petshare.Domain.Entities.Adopter", "Adopter")
                        .WithMany()
                        .HasForeignKey("AdopterID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Petshare.Domain.Entities.Annoucement", "annoucement")
                        .WithMany()
                        .HasForeignKey("AnnoucementID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Adopter");

                    b.Navigation("annoucement");
                });

            modelBuilder.Entity("Petshare.Domain.Entities.Annoucement", b =>
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
                    b.HasOne("Petshare.Domain.Entities.Annoucement", "Annoucement")
                        .WithMany()
                        .HasForeignKey("AnnoucementID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Petshare.Domain.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Annoucement");

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

                    b.OwnsOne("Petshare.Domain.Entities.AnnoucementProvider", "AnnoucementProvider", b1 =>
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

                    b.Navigation("AnnoucementProvider")
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
