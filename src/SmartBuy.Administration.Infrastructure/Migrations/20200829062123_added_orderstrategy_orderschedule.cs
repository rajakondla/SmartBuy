using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.Administration.Infrastructure.Migrations
{
    public partial class added_orderstrategy_orderschedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "FromTime",
                schema: "Administrator",
                table: "GasStations",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "ToTime",
                schema: "Administrator",
                table: "GasStations",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateTable(
                name: "GasStationScheduleByDays",
                schema: "Administrator",
                columns: table => new
                {
                    GasStationId = table.Column<Guid>(nullable: false),
                    DayOfWeek = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_GasStationScheduleByDays_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalSchema: "Administrator",
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasStationScheduleByTimes",
                schema: "Administrator",
                columns: table => new
                {
                    GasStationId = table.Column<Guid>(nullable: false),
                    TimeInteral = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_GasStationScheduleByTimes_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalSchema: "Administrator",
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasStationSchedules",
                schema: "Administrator",
                columns: table => new
                {
                    GasStationId = table.Column<Guid>(nullable: false),
                    ScheduleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_GasStationSchedules_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalSchema: "Administrator",
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GasStationTankSchedule",
                schema: "Administrator",
                columns: table => new
                {
                    TankId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Unit = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_GasStationTankSchedule_Tanks_TankId",
                        column: x => x.TankId,
                        principalSchema: "Administrator",
                        principalTable: "Tanks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderStrategies",
                schema: "Administrator",
                columns: table => new
                {
                    GasStationId = table.Column<Guid>(nullable: false),
                    CreationType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_OrderStrategies_GasStations_GasStationId",
                        column: x => x.GasStationId,
                        principalSchema: "Administrator",
                        principalTable: "GasStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GasStationScheduleByDays_GasStationId",
                schema: "Administrator",
                table: "GasStationScheduleByDays",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStationScheduleByTimes_GasStationId",
                schema: "Administrator",
                table: "GasStationScheduleByTimes",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStationSchedules_GasStationId",
                schema: "Administrator",
                table: "GasStationSchedules",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStationTankSchedule_TankId",
                schema: "Administrator",
                table: "GasStationTankSchedule",
                column: "TankId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStrategies_GasStationId",
                schema: "Administrator",
                table: "OrderStrategies",
                column: "GasStationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GasStationScheduleByDays",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "GasStationScheduleByTimes",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "GasStationSchedules",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "GasStationTankSchedule",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "OrderStrategies",
                schema: "Administrator");

            migrationBuilder.DropColumn(
                name: "FromTime",
                schema: "Administrator",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "ToTime",
                schema: "Administrator",
                table: "GasStations");
        }
    }
}
