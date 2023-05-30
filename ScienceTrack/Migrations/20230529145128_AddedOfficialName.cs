using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScienceTrack.Migrations
{
    /// <inheritdoc />
    public partial class AddedOfficialName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "officialName",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "officialName",
                table: "Users");
        }
    }
}
