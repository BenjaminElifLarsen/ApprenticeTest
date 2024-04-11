using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Shared.Migrations
{
    /// <inheritdoc />
    public partial class menu_item_price : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MenuPart",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "MenuPart");
        }
    }
}
