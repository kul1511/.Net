using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CROPDEAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedOrderNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crops_Orders_OrderId",
                table: "Crops");

            migrationBuilder.DropIndex(
                name: "IX_Crops_OrderId",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Crops");

            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "OrderId",
                table: "Crops",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Crops_OrderId",
                table: "Crops",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_Orders_OrderId",
                table: "Crops",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }
    }
}
