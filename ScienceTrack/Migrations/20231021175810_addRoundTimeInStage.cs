using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScienceTrack.Migrations
{
    public partial class addRoundTimeInStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundDuration",
                table: "Stages",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundDuration",
                table: "Stages");
        }
    }
}
