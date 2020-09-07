using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.OrderManagement.Infrastructure.Migrations
{
    public partial class order_dispatchdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ToTime",
                schema: "OrderManagement",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FromTime",
                schema: "OrderManagement",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ToTime",
                schema: "OrderManagement",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FromTime",
                schema: "OrderManagement",
                table: "Orders");
        }
    }
}
