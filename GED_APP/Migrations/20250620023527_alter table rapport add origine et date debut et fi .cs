using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertablerapportaddorigineetdatedebutetfi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Correspondances_TypeCorresps_TypeCorrespId",
                table: "Correspondances");

            migrationBuilder.DropTable(
                name: "TypeCorresps");

            migrationBuilder.DropIndex(
                name: "IX_Correspondances_TypeCorrespId",
                table: "Correspondances");

            migrationBuilder.DropColumn(
                name: "TypeCorrespId",
                table: "Correspondances");

            migrationBuilder.AddColumn<string>(
                name: "Origine",
                table: "Correspondances",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Correspondances",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origine",
                table: "Correspondances");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Correspondances");

            migrationBuilder.AddColumn<int>(
                name: "TypeCorrespId",
                table: "Correspondances",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TypeCorresps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Libele = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Updated = table.Column<DateTime>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeCorresps", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Correspondances_TypeCorrespId",
                table: "Correspondances",
                column: "TypeCorrespId");

            migrationBuilder.AddForeignKey(
                name: "FK_Correspondances_TypeCorresps_TypeCorrespId",
                table: "Correspondances",
                column: "TypeCorrespId",
                principalTable: "TypeCorresps",
                principalColumn: "Id");
        }
    }
}
