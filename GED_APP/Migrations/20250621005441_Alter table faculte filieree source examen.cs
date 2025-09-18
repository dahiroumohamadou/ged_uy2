using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GED_APP.Migrations
{
    public partial class Altertablefacultefiliereesourceexamen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaculteId",
                table: "PvExamens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Facultes",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Examens",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_PvExamens_FaculteId",
                table: "PvExamens",
                column: "FaculteId");

            migrationBuilder.AddForeignKey(
                name: "FK_PvExamens_Facultes_FaculteId",
                table: "PvExamens",
                column: "FaculteId",
                principalTable: "Facultes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PvExamens_Facultes_FaculteId",
                table: "PvExamens");

            migrationBuilder.DropIndex(
                name: "IX_PvExamens_FaculteId",
                table: "PvExamens");

            migrationBuilder.DropColumn(
                name: "FaculteId",
                table: "PvExamens");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Facultes");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Examens");
        }
    }
}
