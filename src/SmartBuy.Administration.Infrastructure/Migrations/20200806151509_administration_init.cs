using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.Administration.Infrastructure.Migrations
{
    public partial class administration_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Administrator");

            migrationBuilder.CreateTable(
                name: "GasStations",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GasStations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tanks",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Number = table.Column<int>(nullable: false),
                    GasStationId = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<int>(nullable: false),
                    NetQuantity = table.Column<int>(nullable: true),
                    Unit = table.Column<int>(nullable: true),
                    Top = table.Column<int>(nullable: false),
                    Bottom = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tanks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tanks_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalSchema: "Administrator",
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tanks_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "Administrator",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TankReadings",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TankId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    ReadingTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TankReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TankReadings_Tanks_TankId",
                        column: x => x.TankId,
                        principalSchema: "Administrator",
                        principalTable: "Tanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TankSales",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(nullable: false),
                    SaleTime = table.Column<DateTime>(nullable: false),
                    TankId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TankSales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TankSales_Tanks_TankId",
                        column: x => x.TankId,
                        principalSchema: "Administrator",
                        principalTable: "Tanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TankReadings_TankId",
                schema: "Administrator",
                table: "TankReadings",
                column: "TankId");

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_GasStationId",
                schema: "Administrator",
                table: "Tanks",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Tanks_ProductId",
                schema: "Administrator",
                table: "Tanks",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TankSales_TankId",
                schema: "Administrator",
                table: "TankSales",
                column: "TankId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TankReadings",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "TankSales",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "Tanks",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "GasStations",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "Administrator");
        }
    }
}
