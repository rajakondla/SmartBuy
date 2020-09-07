using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartBuy.Administration.Infrastructure.Migrations
{
    public partial class added_estimatedTank_Column : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EstimatedDaySale",
                schema: "Administrator",
                table: "Tanks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedDaySale",
                schema: "Administrator",
                table: "Tanks");
        }
    }
}
