using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScienceTrack.Migrations
{
    public partial class AddRelationStageForRound : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "Rounds",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_Stage",
                table: "Rounds",
                column: "Stage");

            migrationBuilder.AddForeignKey(
                name: "Rounds_stage_fkey",
                table: "Rounds",
                column: "Stage",
                principalTable: "Stages",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Rounds_stage_fkey",
                table: "Rounds");

            migrationBuilder.DropIndex(
                name: "IX_Rounds_Stage",
                table: "Rounds");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Rounds");
        }
    }
}
