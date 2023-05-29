using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ScienceTrack.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    status = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Games_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "GlobalEvents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    socialStatus = table.Column<int>(type: "integer", nullable: true),
                    financeStatus = table.Column<int>(type: "integer", nullable: true),
                    administrativeStatus = table.Column<int>(type: "integer", nullable: true),
                    chance = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GlobalEvents_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LocalEvents",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    socialStatus = table.Column<int>(type: "integer", nullable: true),
                    financeStatus = table.Column<int>(type: "integer", nullable: true),
                    administrativeStatus = table.Column<int>(type: "integer", nullable: true),
                    chance = table.Column<decimal>(type: "numeric", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LocalEvents_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LocalSolutions",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "text", nullable: false),
                    socialStatus = table.Column<int>(type: "integer", nullable: true),
                    financeStatus = table.Column<int>(type: "integer", nullable: true),
                    administrativeStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("LocalSolutions_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    roleName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Roles_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "text", nullable: true),
                    globalEvent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Rounds_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Rounds_game_fkey",
                        column: x => x.game,
                        principalTable: "Games",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "Rounds_globalEvent_fkey",
                        column: x => x.globalEvent,
                        principalTable: "GlobalEvents",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userName = table.Column<string>(type: "text", nullable: false),
                    passwordHash = table.Column<string>(type: "text", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Users_pkey", x => x.id);
                    table.ForeignKey(
                        name: "Users_role_fkey",
                        column: x => x.role,
                        principalTable: "Roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "GameUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    game = table.Column<int>(type: "integer", nullable: false),
                    user = table.Column<int>(type: "integer", nullable: false),
                    socialStatus = table.Column<int>(type: "integer", nullable: true),
                    financeStatus = table.Column<int>(type: "integer", nullable: true),
                    administrativeStatus = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("GameUsers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "GameUsers_game_fkey",
                        column: x => x.game,
                        principalTable: "Games",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "GameUsers_user_fkey",
                        column: x => x.user,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "RoundUsers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    round = table.Column<int>(type: "integer", nullable: false),
                    user = table.Column<int>(type: "integer", nullable: false),
                    localSolution = table.Column<int>(type: "integer", nullable: true),
                    localEvent = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RoundUsers_pkey", x => x.id);
                    table.ForeignKey(
                        name: "RoundUsers_localEvent_fkey",
                        column: x => x.localEvent,
                        principalTable: "LocalEvents",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "RoundUsers_localSolution_fkey",
                        column: x => x.localSolution,
                        principalTable: "LocalSolutions",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "RoundUsers_round_fkey",
                        column: x => x.round,
                        principalTable: "Rounds",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "RoundUsers_user_fkey",
                        column: x => x.user,
                        principalTable: "Users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameUsers_game",
                table: "GameUsers",
                column: "game");

            migrationBuilder.CreateIndex(
                name: "IX_GameUsers_user",
                table: "GameUsers",
                column: "user");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_game",
                table: "Rounds",
                column: "game");

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_globalEvent",
                table: "Rounds",
                column: "globalEvent");

            migrationBuilder.CreateIndex(
                name: "IX_RoundUsers_localEvent",
                table: "RoundUsers",
                column: "localEvent");

            migrationBuilder.CreateIndex(
                name: "IX_RoundUsers_localSolution",
                table: "RoundUsers",
                column: "localSolution");

            migrationBuilder.CreateIndex(
                name: "IX_RoundUsers_round",
                table: "RoundUsers",
                column: "round");

            migrationBuilder.CreateIndex(
                name: "IX_RoundUsers_user",
                table: "RoundUsers",
                column: "user");

            migrationBuilder.CreateIndex(
                name: "IX_Users_role",
                table: "Users",
                column: "role");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameUsers");

            migrationBuilder.DropTable(
                name: "RoundUsers");

            migrationBuilder.DropTable(
                name: "LocalEvents");

            migrationBuilder.DropTable(
                name: "LocalSolutions");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "GlobalEvents");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
