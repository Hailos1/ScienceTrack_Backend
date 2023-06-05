using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScienceTrack.Migrations
{
    /// <inheritdoc />
    public partial class addstages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stage",
                table: "Games",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stage",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    desc = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Stages_pkey", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_stage",
                table: "Games",
                column: "stage");

            migrationBuilder.AddForeignKey(
                name: "Game_stage_fkey",
                table: "Games",
                column: "stage",
                principalTable: "Stage",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "Game_stage_fkey",
                table: "Games");

            migrationBuilder.DropTable(
                name: "Stage");

            migrationBuilder.DropIndex(
                name: "IX_Games_stage",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "stage",
                table: "Games");
        }
    }
}
