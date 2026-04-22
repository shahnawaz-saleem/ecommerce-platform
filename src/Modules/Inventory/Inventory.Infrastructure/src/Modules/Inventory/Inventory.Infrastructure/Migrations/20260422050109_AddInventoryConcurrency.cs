using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.src.Modules.Inventory.Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddInventoryConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "InventoryItems",
                newName: "TotalStock");

            migrationBuilder.AddColumn<int>(
                name: "ReservedStock",
                table: "InventoryItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "InventoryItems",
                type: "bytea",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservedStock",
                table: "InventoryItems");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "InventoryItems");

            migrationBuilder.RenameColumn(
                name: "TotalStock",
                table: "InventoryItems",
                newName: "Quantity");
        }
    }
}
