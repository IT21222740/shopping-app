using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Changeuserproducttablename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProducts_Products_ProductId",
                table: "ProductProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProducts_Users_userId",
                table: "ProductProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductProducts",
                table: "ProductProducts");

            migrationBuilder.RenameTable(
                name: "ProductProducts",
                newName: "UserProducts");

            migrationBuilder.RenameIndex(
                name: "IX_ProductProducts_ProductId",
                table: "UserProducts",
                newName: "IX_UserProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts",
                columns: new[] { "userId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_Products_ProductId",
                table: "UserProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProducts_Users_userId",
                table: "UserProducts",
                column: "userId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_Products_ProductId",
                table: "UserProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProducts_Users_userId",
                table: "UserProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProducts",
                table: "UserProducts");

            migrationBuilder.RenameTable(
                name: "UserProducts",
                newName: "ProductProducts");

            migrationBuilder.RenameIndex(
                name: "IX_UserProducts_ProductId",
                table: "ProductProducts",
                newName: "IX_ProductProducts_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductProducts",
                table: "ProductProducts",
                columns: new[] { "userId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProducts_Products_ProductId",
                table: "ProductProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProducts_Users_userId",
                table: "ProductProducts",
                column: "userId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
