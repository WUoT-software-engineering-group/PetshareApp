using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    /// <inheritdoc />
    public partial class addpetstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Pets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Pets");
        }
    }
}
