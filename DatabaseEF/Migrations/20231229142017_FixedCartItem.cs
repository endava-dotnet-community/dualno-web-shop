using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseEF.Migrations
{
    /// <inheritdoc />
    public partial class FixedCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductId",
                table: "ShoppingCartItem");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ShoppingCartItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductId",
                table: "ShoppingCartItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductId",
                table: "ShoppingCartItem");

            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ShoppingCartItem",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductId",
                table: "ShoppingCartItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
