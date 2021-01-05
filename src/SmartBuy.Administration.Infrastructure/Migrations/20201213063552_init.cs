using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.Administration.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStrategies_GasStationId",
                schema: "Administrator",
                table: "OrderStrategies");

            migrationBuilder.DropIndex(
                name: "IX_GasStationSchedules_GasStationId",
                schema: "Administrator",
                table: "GasStationSchedules");

            migrationBuilder.DropIndex(
                name: "IX_GasStationScheduleByTimes_GasStationId",
                schema: "Administrator",
                table: "GasStationScheduleByTimes");

            migrationBuilder.DropIndex(
                name: "IX_GasStationScheduleByDays_GasStationId",
                schema: "Administrator",
                table: "GasStationScheduleByDays");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                schema: "Administrator",
                table: "GasStationTankSchedule",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ToTime",
                schema: "Administrator",
                table: "GasStations",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "FromTime",
                schema: "Administrator",
                table: "GasStations",
                nullable: true,
                oldClrType: typeof(TimeSpan),
                oldType: "time");

            migrationBuilder.AddColumn<Guid>(
                name: "DispatcherGroupId",
                schema: "Administrator",
                table: "GasStations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStrategies",
                schema: "Administrator",
                table: "OrderStrategies",
                columns: new[] { "GasStationId", "OrderType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GasStationTankSchedule",
                schema: "Administrator",
                table: "GasStationTankSchedule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GasStationSchedules",
                schema: "Administrator",
                table: "GasStationSchedules",
                columns: new[] { "GasStationId", "ScheduleType" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GasStationScheduleByTimes",
                schema: "Administrator",
                table: "GasStationScheduleByTimes",
                columns: new[] { "GasStationId", "TimeInteral" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_GasStationScheduleByDays",
                schema: "Administrator",
                table: "GasStationScheduleByDays",
                columns: new[] { "GasStationId", "DayOfWeek" });

            migrationBuilder.CreateTable(
                name: "DispatcherGroup",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatcherGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dispatcher",
                schema: "Administrator",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    DispatcherGroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dispatcher", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Dispatcher_DispatcherGroup_DispatcherGroupId",
                        column: x => x.DispatcherGroupId,
                        principalSchema: "Administrator",
                        principalTable: "DispatcherGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GasStations_DispatcherGroupId",
                schema: "Administrator",
                table: "GasStations",
                column: "DispatcherGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Dispatcher_DispatcherGroupId",
                schema: "Administrator",
                table: "Dispatcher",
                column: "DispatcherGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_GasStations_DispatcherGroup_DispatcherGroupId",
                schema: "Administrator",
                table: "GasStations",
                column: "DispatcherGroupId",
                principalSchema: "Administrator",
                principalTable: "DispatcherGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GasStations_DispatcherGroup_DispatcherGroupId",
                schema: "Administrator",
                table: "GasStations");

            migrationBuilder.DropTable(
                name: "Dispatcher",
                schema: "Administrator");

            migrationBuilder.DropTable(
                name: "DispatcherGroup",
                schema: "Administrator");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStrategies",
                schema: "Administrator",
                table: "OrderStrategies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GasStationTankSchedule",
                schema: "Administrator",
                table: "GasStationTankSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GasStationSchedules",
                schema: "Administrator",
                table: "GasStationSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GasStationScheduleByTimes",
                schema: "Administrator",
                table: "GasStationScheduleByTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GasStationScheduleByDays",
                schema: "Administrator",
                table: "GasStationScheduleByDays");

            migrationBuilder.DropIndex(
                name: "IX_GasStations_DispatcherGroupId",
                schema: "Administrator",
                table: "GasStations");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "Administrator",
                table: "GasStationTankSchedule");

            migrationBuilder.DropColumn(
                name: "DispatcherGroupId",
                schema: "Administrator",
                table: "GasStations");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "FromTime",
                schema: "Administrator",
                table: "GasStations",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "ToTime",
                schema: "Administrator",
                table: "GasStations",
                type: "time",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStrategies_GasStationId",
                schema: "Administrator",
                table: "OrderStrategies",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStationSchedules_GasStationId",
                schema: "Administrator",
                table: "GasStationSchedules",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStationScheduleByTimes_GasStationId",
                schema: "Administrator",
                table: "GasStationScheduleByTimes",
                column: "GasStationId");

            migrationBuilder.CreateIndex(
                name: "IX_GasStationScheduleByDays_GasStationId",
                schema: "Administrator",
                table: "GasStationScheduleByDays",
                column: "GasStationId");
        }
    }
}
