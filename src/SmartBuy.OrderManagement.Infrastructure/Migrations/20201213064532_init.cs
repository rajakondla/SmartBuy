using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.OrderManagement.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FromTime",
                schema: "OrderManagement",
                table: "Orders",
                newName: "FromDateTime");

            migrationBuilder.RenameColumn(
                name: "ToTime",
                schema: "OrderManagement",
                table: "Orders",
                newName: "ToDateTime");

            migrationBuilder.AddColumn<Guid>(
                name: "CarrierId",
                schema: "OrderManagement",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderType",
                schema: "OrderManagement",
                table: "Orders",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarrierId",
                schema: "OrderManagement",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderType",
                schema: "OrderManagement",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "FromDateTime",
                schema: "OrderManagement",
                table: "Orders",
                newName: "FromTime");

            migrationBuilder.RenameColumn(
                name: "ToDateTime",
                schema: "OrderManagement",
                table: "Orders",
                newName: "ToTime");
        }
    }
}
