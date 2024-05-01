using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Promises.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_migration_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AgreementUsers_AgreementId",
                table: "AgreementUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementUsers_AgreementId",
                table: "AgreementUsers",
                column: "AgreementId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AgreementUsers_AgreementId",
                table: "AgreementUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AgreementUsers_AgreementId",
                table: "AgreementUsers",
                column: "AgreementId",
                unique: true);
        }
    }
}
