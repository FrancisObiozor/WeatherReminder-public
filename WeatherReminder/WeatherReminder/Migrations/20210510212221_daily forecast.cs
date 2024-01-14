using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class dailyforecast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DailyForecast",
                table: "WeatherSettings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyForecast",
                table: "WeatherSettings");
        }
    }
}
