using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coney.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserXRiffles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserXRiffles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiffleId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserXRiffles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserXRiffles_Riffles_RiffleId",
                        column: x => x.RiffleId,
                        principalTable: "Riffles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserXRiffles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserXRiffles_RiffleId_UserId",
                table: "UserXRiffles",
                columns: new[] { "RiffleId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserXRiffles_UserId",
                table: "UserXRiffles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserXRiffles");
        }
    }
}
