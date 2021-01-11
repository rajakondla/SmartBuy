using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace SmartBuy.OrderManagement.Infrastructure.Migrations
{
    public partial class order_datetime_isrequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
              name: "FromDateTime",
              schema: "OrderManagement",
              table: "Orders",
              nullable: false);

            migrationBuilder.AlterColumn<DateTime>(
               name: "ToDateTime",
               schema: "OrderManagement",
               table: "Orders",
               nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
