using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    /// <inheritdoc />
    public partial class addapplicationstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWithdrawn",
                table: "Applications");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Applications",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Applications");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithdrawn",
                table: "Applications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
