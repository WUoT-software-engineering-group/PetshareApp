using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    /// <inheritdoc />
    public partial class addshelteradopterverificationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FinalizedAnnouncements_Adopters_AdopterID",
                table: "FinalizedAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_FinalizedAnnouncements_Announcements_AnnouncementID",
                table: "FinalizedAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowedAnnouncements_Adopters_AdopterID",
                table: "FollowedAnnouncements");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowedAnnouncements_Announcements_AnnouncementID",
                table: "FollowedAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_FollowedAnnouncements_AnnouncementID",
                table: "FollowedAnnouncements");

            migrationBuilder.DropIndex(
                name: "IX_FinalizedAnnouncements_AnnouncementID",
                table: "FinalizedAnnouncements");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Adopters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VerifiedAdopters",
                columns: table => new
                {
                    ShelterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdopterID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifiedAdopters", x => new { x.ShelterID, x.AdopterID });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerifiedAdopters");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Adopters");

            migrationBuilder.CreateIndex(
                name: "IX_FollowedAnnouncements_AnnouncementID",
                table: "FollowedAnnouncements",
                column: "AnnouncementID");

            migrationBuilder.CreateIndex(
                name: "IX_FinalizedAnnouncements_AnnouncementID",
                table: "FinalizedAnnouncements",
                column: "AnnouncementID");

            migrationBuilder.AddForeignKey(
                name: "FK_FinalizedAnnouncements_Adopters_AdopterID",
                table: "FinalizedAnnouncements",
                column: "AdopterID",
                principalTable: "Adopters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FinalizedAnnouncements_Announcements_AnnouncementID",
                table: "FinalizedAnnouncements",
                column: "AnnouncementID",
                principalTable: "Announcements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedAnnouncements_Adopters_AdopterID",
                table: "FollowedAnnouncements",
                column: "AdopterID",
                principalTable: "Adopters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowedAnnouncements_Announcements_AnnouncementID",
                table: "FollowedAnnouncements",
                column: "AnnouncementID",
                principalTable: "Announcements",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
