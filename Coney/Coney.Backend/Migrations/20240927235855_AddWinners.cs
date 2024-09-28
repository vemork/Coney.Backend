using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coney.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddWinners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Winners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Observations = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WasDelivered = table.Column<bool>(type: "bit", nullable: false),
                    PrizeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RiffleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Winners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Winners_Prices_PrizeId",
                        column: x => x.PrizeId,
                        principalTable: "Prices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Winners_Riffles_RiffleId",
                        column: x => x.RiffleId,
                        principalTable: "Riffles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Winners_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Winners_PrizeId_UserId_RiffleId",
                table: "Winners",
                columns: new[] { "PrizeId", "UserId", "RiffleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Winners_RiffleId",
                table: "Winners",
                column: "RiffleId");

            migrationBuilder.CreateIndex(
                name: "IX_Winners_UserId",
                table: "Winners",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Winners");
        }
    }
}
