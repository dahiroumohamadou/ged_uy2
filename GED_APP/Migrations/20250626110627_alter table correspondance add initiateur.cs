using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertablecorrespondanceaddinitiateur : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "structureInit",
                table: "Correspondances",
                newName: "Initiateur");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Initiateur",
                table: "Correspondances",
                newName: "structureInit");
        }
    }
}
