using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Petshare.DataPersistence.Migrations
{
    /// <inheritdoc />
    public partial class changepetphotocolumntype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Photo",
                table: "Pets");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUri",
                table: "Pets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoUri",
                table: "Pets");

            migrationBuilder.AddColumn<byte[]>(
                name: "Photo",
                table: "Pets",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
