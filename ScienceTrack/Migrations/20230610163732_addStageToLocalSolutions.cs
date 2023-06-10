using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScienceTrack.Migrations
{
    public partial class addStageToLocalSolutions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stage",
                table: "LocalSolutions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalSolutions_stage",
                table: "LocalSolutions",
                column: "stage");

            migrationBuilder.AddForeignKey(
                name: "LocalSolution_stage_fkey",
                table: "LocalSolutions",
                column: "stage",
                principalTable: "Stages",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "LocalSolution_stage_fkey",
                table: "LocalSolutions");

            migrationBuilder.DropIndex(
                name: "IX_LocalSolutions_stage",
                table: "LocalSolutions");

            migrationBuilder.DropColumn(
                name: "stage",
                table: "LocalSolutions");
        }
    }
}
