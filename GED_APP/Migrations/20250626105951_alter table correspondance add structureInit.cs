using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertablecorrespondanceaddstructureInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "structureInit",
                table: "Correspondances",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "structureInit",
                table: "Correspondances");
        }
    }
}
