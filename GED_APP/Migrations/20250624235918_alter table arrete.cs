using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertablearrete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "numeroCNE",
                table: "Arretes",
                newName: "NumeroCNE");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroCNE",
                table: "Arretes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DateCne",
                table: "Arretes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Origine",
                table: "Arretes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCne",
                table: "Arretes");

            migrationBuilder.DropColumn(
                name: "Origine",
                table: "Arretes");

            migrationBuilder.RenameColumn(
                name: "NumeroCNE",
                table: "Arretes",
                newName: "numeroCNE");

            migrationBuilder.AlterColumn<int>(
                name: "numeroCNE",
                table: "Arretes",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
