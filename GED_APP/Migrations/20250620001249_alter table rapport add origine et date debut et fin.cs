using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertablerapportaddorigineetdatedebutetfin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Periode",
                table: "Rapports",
                newName: "Origine");

            migrationBuilder.AddColumn<string>(
                name: "Debut",
                table: "Rapports",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Fin",
                table: "Rapports",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Debut",
                table: "Rapports");

            migrationBuilder.DropColumn(
                name: "Fin",
                table: "Rapports");

            migrationBuilder.RenameColumn(
                name: "Origine",
                table: "Rapports",
                newName: "Periode");
        }
    }
}
