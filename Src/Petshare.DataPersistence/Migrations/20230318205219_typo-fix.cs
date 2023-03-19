using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    /// <inheritdoc />
    public partial class typofix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Annoucements_AnnoucementID",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "FinalisedAnnoucements");

            migrationBuilder.DropTable(
                name: "FollowedAnnoucements");

            migrationBuilder.DropTable(
                name: "Annoucements");

            migrationBuilder.RenameColumn(
                name: "AnnoucementID",
                table: "Reports",
                newName: "AnnouncementID");

            migrationBuilder.RenameColumn(
                name: "AnnoucementID",
                table: "Applications",
                newName: "AnnouncementID");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_AnnoucementID",
                table: "Applications",
                newName: "IX_Applications_AnnouncementID");

            migrationBuilder.CreateTable(
                name: "Announcements",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Announcements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Announcements_Pets_PetID",
                        column: x => x.PetID,
                        principalTable: "Pets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Announcements_Shelters_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Shelters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinalizedAnnouncements",
                columns: table => new
                {
                    AdopterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalizedAnnouncements", x => new { x.AdopterID, x.AnnouncementID });
                    table.ForeignKey(
                        name: "FK_FinalizedAnnouncements_Adopters_AdopterID",
                        column: x => x.AdopterID,
                        principalTable: "Adopters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalizedAnnouncements_Announcements_AnnouncementID",
                        column: x => x.AnnouncementID,
                        principalTable: "Announcements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FollowedAnnouncements",
                columns: table => new
                {
                    AdopterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnouncementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedAnnouncements", x => new { x.AdopterID, x.AnnouncementID });
                    table.ForeignKey(
                        name: "FK_FollowedAnnouncements_Adopters_AdopterID",
                        column: x => x.AdopterID,
                        principalTable: "Adopters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedAnnouncements_Announcements_AnnouncementID",
                        column: x => x.AnnouncementID,
                        principalTable: "Announcements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_AuthorID",
                table: "Announcements",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Announcements_PetID",
                table: "Announcements",
                column: "PetID");

            migrationBuilder.CreateIndex(
                name: "IX_FinalizedAnnouncements_AnnouncementID",
                table: "FinalizedAnnouncements",
                column: "AnnouncementID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedAnnouncements_AnnouncementID",
                table: "FollowedAnnouncements",
                column: "AnnouncementID");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Announcements_AnnouncementID",
                table: "Applications",
                column: "AnnouncementID",
                principalTable: "Announcements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Announcements_AnnouncementID",
                table: "Applications");

            migrationBuilder.DropTable(
                name: "FinalizedAnnouncements");

            migrationBuilder.DropTable(
                name: "FollowedAnnouncements");

            migrationBuilder.DropTable(
                name: "Announcements");

            migrationBuilder.RenameColumn(
                name: "AnnouncementID",
                table: "Reports",
                newName: "AnnoucementID");

            migrationBuilder.RenameColumn(
                name: "AnnouncementID",
                table: "Applications",
                newName: "AnnoucementID");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_AnnouncementID",
                table: "Applications",
                newName: "IX_Applications_AnnoucementID");

            migrationBuilder.CreateTable(
                name: "Annoucements",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClosingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annoucements", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Annoucements_Pets_PetID",
                        column: x => x.PetID,
                        principalTable: "Pets",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Annoucements_Shelters_AuthorID",
                        column: x => x.AuthorID,
                        principalTable: "Shelters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FinalisedAnnoucements",
                columns: table => new
                {
                    AdopterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnoucementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalisedAnnoucements", x => new { x.AdopterID, x.AnnoucementID });
                    table.ForeignKey(
                        name: "FK_FinalisedAnnoucements_Adopters_AdopterID",
                        column: x => x.AdopterID,
                        principalTable: "Adopters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FinalisedAnnoucements_Annoucements_AnnoucementID",
                        column: x => x.AnnoucementID,
                        principalTable: "Annoucements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FollowedAnnoucements",
                columns: table => new
                {
                    AdopterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnoucementID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowedAnnoucements", x => new { x.AdopterID, x.AnnoucementID });
                    table.ForeignKey(
                        name: "FK_FollowedAnnoucements_Adopters_AdopterID",
                        column: x => x.AdopterID,
                        principalTable: "Adopters",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FollowedAnnoucements_Annoucements_AnnoucementID",
                        column: x => x.AnnoucementID,
                        principalTable: "Annoucements",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annoucements_AuthorID",
                table: "Annoucements",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_Annoucements_PetID",
                table: "Annoucements",
                column: "PetID");

            migrationBuilder.CreateIndex(
                name: "IX_FinalisedAnnoucements_AnnoucementID",
                table: "FinalisedAnnoucements",
                column: "AnnoucementID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedAnnoucements_AnnoucementID",
                table: "FollowedAnnoucements",
                column: "AnnoucementID");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Annoucements_AnnoucementID",
                table: "Applications",
                column: "AnnoucementID",
                principalTable: "Annoucements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
