using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class altertableliste : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Listes",
                newName: "Origine");

            migrationBuilder.AlterColumn<string>(
                name: "NumeroCne",
                table: "Listes",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DateCne",
                table: "Listes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "DateSign",
                table: "Listes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Listes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "StructureId",
                table: "Listes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listes_StructureId",
                table: "Listes",
                column: "StructureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listes_Structures_StructureId",
                table: "Listes",
                column: "StructureId",
                principalTable: "Structures",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listes_Structures_StructureId",
                table: "Listes");

            migrationBuilder.DropIndex(
                name: "IX_Listes_StructureId",
                table: "Listes");

            migrationBuilder.DropColumn(
                name: "DateCne",
                table: "Listes");

            migrationBuilder.DropColumn(
                name: "DateSign",
                table: "Listes");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Listes");

            migrationBuilder.DropColumn(
                name: "StructureId",
                table: "Listes");

            migrationBuilder.RenameColumn(
                name: "Origine",
                table: "Listes",
                newName: "Type");

            migrationBuilder.AlterColumn<int>(
                name: "NumeroCne",
                table: "Listes",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
