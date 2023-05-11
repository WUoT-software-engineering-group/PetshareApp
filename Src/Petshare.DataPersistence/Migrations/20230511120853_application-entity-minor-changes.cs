using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    /// <inheritdoc />
    public partial class applicationentityminorchanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Users_UserID",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Applications",
                newName: "AdopterID");

            migrationBuilder.RenameColumn(
                name: "DateOfApplication",
                table: "Applications",
                newName: "LastUpdateDate");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_UserID",
                table: "Applications",
                newName: "IX_Applications_AdopterID");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Applications",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Adopters_AdopterID",
                table: "Applications",
                column: "AdopterID",
                principalTable: "Adopters",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_Adopters_AdopterID",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Applications");

            migrationBuilder.RenameColumn(
                name: "LastUpdateDate",
                table: "Applications",
                newName: "DateOfApplication");

            migrationBuilder.RenameColumn(
                name: "AdopterID",
                table: "Applications",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Applications_AdopterID",
                table: "Applications",
                newName: "IX_Applications_UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_Users_UserID",
                table: "Applications",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
