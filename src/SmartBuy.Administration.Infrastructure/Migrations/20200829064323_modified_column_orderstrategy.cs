using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.Administration.Infrastructure.Migrations
{
    public partial class modified_column_orderstrategy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationType",
                schema: "Administrator",
                table: "OrderStrategies");

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                schema: "Administrator",
                table: "OrderStrategies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderType",
                schema: "Administrator",
                table: "OrderStrategies");

            migrationBuilder.AddColumn<int>(
                name: "CreationType",
                schema: "Administrator",
                table: "OrderStrategies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
