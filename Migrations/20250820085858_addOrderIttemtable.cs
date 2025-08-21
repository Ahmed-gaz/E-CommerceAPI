using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_CommerceAPI.Migrations
{
    /// <inheritdoc />
    public partial class addOrderIttemtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "cartItems");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "cartItems",
                newName: "IX_cartItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_CartId",
                table: "cartItems",
                newName: "IX_cartItems_CartId");

            migrationBuilder.AlterColumn<string>(
                name: "TransactionId",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Method",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_cartItems",
                table: "cartItems",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_cartItems_Carts_CartId",
                table: "cartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_cartItems_Products_ProductId",
                table: "cartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cartItems_Carts_CartId",
                table: "cartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_cartItems_Products_ProductId",
                table: "cartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_cartItems",
                table: "cartItems");

            migrationBuilder.DropColumn(
                name: "Method",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Payments");

            migrationBuilder.RenameTable(
                name: "cartItems",
                newName: "CartItems");

            migrationBuilder.RenameIndex(
                name: "IX_cartItems_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_cartItems_CartId",
                table: "CartItems",
                newName: "IX_CartItems_CartId");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionId",
                table: "Payments",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "PaymentMethod",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentStatus",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId",
                table: "CartItems",
                column: "CartId",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Products_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
