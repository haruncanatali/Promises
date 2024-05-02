using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promises.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fire_file_config_added : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceToken",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "EntityStatus",
                table: "Agreements",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Agreements",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EntityStatus",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Agreements");
        }
    }
}
