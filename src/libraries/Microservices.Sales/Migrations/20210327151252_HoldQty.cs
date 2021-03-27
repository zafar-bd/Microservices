using Microsoft.EntityFrameworkCore.Migrations;

namespace Microservices.Sales.Migrations
{
    public partial class HoldQty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HoldQty",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HoldQty",
                table: "Products");
        }
    }
}
