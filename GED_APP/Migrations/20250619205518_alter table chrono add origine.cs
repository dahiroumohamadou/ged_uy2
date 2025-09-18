using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertablechronoaddorigine : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Origine",
                table: "Chronogrammes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Origine",
                table: "Chronogrammes");
        }
    }
}
