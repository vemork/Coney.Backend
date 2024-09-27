using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coney.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRifflexRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RiffleXRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RiffleId = table.Column<int>(type: "int", nullable: false),
                    RuleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiffleXRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RiffleXRules_Riffles_RiffleId",
                        column: x => x.RiffleId,
                        principalTable: "Riffles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RiffleXRules_Rules_RuleId",
                        column: x => x.RuleId,
                        principalTable: "Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RiffleXRules_RiffleId_RuleId",
                table: "RiffleXRules",
                columns: new[] { "RiffleId", "RuleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiffleXRules_RuleId",
                table: "RiffleXRules",
                column: "RuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiffleXRules");
        }
    }
}
