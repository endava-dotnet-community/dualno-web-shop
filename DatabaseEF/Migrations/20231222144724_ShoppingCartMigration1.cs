using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseEF.Migrations
{
    /// <inheritdoc />
    public partial class ShoppingCartMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_ShoppingCartEntityId",
                table: "CartItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Product_ProductId",
                table: "CartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Carts",
                table: "Carts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_ShoppingCartEntityId",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ShoppingCartEntityId",
                table: "CartItems");

            migrationBuilder.RenameTable(
                name: "Carts",
                newName: "ShoppingCart");

            migrationBuilder.RenameTable(
                name: "CartItems",
                newName: "ShoppingCartItem");

            migrationBuilder.RenameIndex(
                name: "IX_CartItems_ProductId",
                table: "ShoppingCartItem",
                newName: "IX_ShoppingCartItem_ProductId");

            migrationBuilder.AddColumn<long>(
                name: "CartId",
                table: "ShoppingCartItem",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCart",
                table: "ShoppingCart",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItem",
                table: "ShoppingCartItem",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItem_CartId",
                table: "ShoppingCartItem",
                column: "CartId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductId",
                table: "ShoppingCartItem",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItem_ShoppingCart_CartId",
                table: "ShoppingCartItem",
                column: "CartId",
                principalTable: "ShoppingCart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_Product_ProductId",
                table: "ShoppingCartItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItem_ShoppingCart_CartId",
                table: "ShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItem",
                table: "ShoppingCartItem");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartItem_CartId",
                table: "ShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCart",
                table: "ShoppingCart");

            migrationBuilder.DropColumn(
                name: "CartId",
                table: "ShoppingCartItem");

            migrationBuilder.RenameTable(
                name: "ShoppingCartItem",
                newName: "CartItems");

            migrationBuilder.RenameTable(
                name: "ShoppingCart",
                newName: "Carts");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItem_ProductId",
                table: "CartItems",
                newName: "IX_CartItems_ProductId");

            migrationBuilder.AddColumn<long>(
                name: "ShoppingCartEntityId",
                table: "CartItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItems",
                table: "CartItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Carts",
                table: "Carts",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartEntityId",
                table: "CartItems",
                column: "ShoppingCartEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_ShoppingCartEntityId",
                table: "CartItems",
                column: "ShoppingCartEntityId",
                principalTable: "Carts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Product_ProductId",
                table: "CartItems",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
