using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservices.Sales.Migrations
{
    public partial class Init_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SoldItems_Products_ProductId",
                table: "SoldItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldItems_Sales_SalesId",
                table: "SoldItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldItems",
                table: "SoldItems");

            migrationBuilder.RenameTable(
                name: "SoldItems",
                newName: "SalesDetails");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItems_SalesId",
                table: "SalesDetails",
                newName: "IX_SalesDetails_SalesId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldItems_ProductId_SalesId",
                table: "SalesDetails",
                newName: "IX_SalesDetails_ProductId_SalesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesDetails",
                table: "SalesDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_Products_ProductId",
                table: "SalesDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesDetails_Sales_SalesId",
                table: "SalesDetails",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_Products_ProductId",
                table: "SalesDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesDetails_Sales_SalesId",
                table: "SalesDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesDetails",
                table: "SalesDetails");

            migrationBuilder.RenameTable(
                name: "SalesDetails",
                newName: "SoldItems");

            migrationBuilder.RenameIndex(
                name: "IX_SalesDetails_SalesId",
                table: "SoldItems",
                newName: "IX_SoldItems_SalesId");

            migrationBuilder.RenameIndex(
                name: "IX_SalesDetails_ProductId_SalesId",
                table: "SoldItems",
                newName: "IX_SoldItems_ProductId_SalesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldItems",
                table: "SoldItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItems_Products_ProductId",
                table: "SoldItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldItems_Sales_SalesId",
                table: "SoldItems",
                column: "SalesId",
                principalTable: "Sales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
