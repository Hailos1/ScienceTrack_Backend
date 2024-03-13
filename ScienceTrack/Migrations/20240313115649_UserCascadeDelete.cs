using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScienceTrack.Migrations
{
    public partial class UserCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "GameUsers_user_fkey",
                table: "GameUsers");

            migrationBuilder.DropForeignKey(
                name: "RoundUsers_user_fkey",
                table: "RoundUsers");

            migrationBuilder.AddForeignKey(
                name: "GameUsers_user_fkey",
                table: "GameUsers",
                column: "user",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "RoundUsers_user_fkey",
                table: "RoundUsers",
                column: "user",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "GameUsers_user_fkey",
                table: "GameUsers");

            migrationBuilder.DropForeignKey(
                name: "RoundUsers_user_fkey",
                table: "RoundUsers");

            migrationBuilder.AddForeignKey(
                name: "GameUsers_user_fkey",
                table: "GameUsers",
                column: "user",
                principalTable: "Users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "RoundUsers_user_fkey",
                table: "RoundUsers",
                column: "user",
                principalTable: "Users",
                principalColumn: "id");
        }
    }
}
