using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Coney.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "role",
                table: "Users");
        }
    }
}
