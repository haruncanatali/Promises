using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Promises.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_migration_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_AspNetUsers_UserId",
                table: "Agreements");

            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_Persons_PersonId",
                table: "Agreements");

            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropIndex(
                name: "IX_Agreements_PersonId",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "CommitmentStatus",
                table: "Agreements");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Agreements");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Agreements",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Agreements",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AgreementUsers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PromiserUserId = table.Column<long>(type: "bigint", nullable: false),
                    PromisedUserId = table.Column<long>(type: "bigint", nullable: false),
                    AgreementId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgreementUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgreementUsers_Agreements_AgreementId",
                        column: x => x.AgreementId,
                        principalTable: "Agreements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgreementUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgreementUsers_AgreementId",
                table: "AgreementUsers",
                column: "AgreementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AgreementUsers_UserId",
                table: "AgreementUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_AspNetUsers_UserId",
                table: "Agreements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Agreements_AspNetUsers_UserId",
                table: "Agreements");

            migrationBuilder.DropTable(
                name: "AgreementUsers");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Agreements");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Agreements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommitmentStatus",
                table: "Agreements",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "PersonId",
                table: "Agreements",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Photo = table.Column<string>(type: "text", nullable: false),
                    Surname = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agreements_PersonId",
                table: "Agreements",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_AspNetUsers_UserId",
                table: "Agreements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Agreements_Persons_PersonId",
                table: "Agreements",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
