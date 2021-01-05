using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.OrderManagement.Infrastructure.Migrations
{
    public partial class addCreatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "OrderManagement",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                schema: "OrderManagement",
                table: "Orders",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "OrderManagement",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                schema: "OrderManagement",
                table: "Orders");
        }
    }
}
