using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class MovedDailyForecastProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyForecast",
                table: "WeatherSettings");

            migrationBuilder.AddColumn<string>(
                name: "DailyForecast",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DailyForecast",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "DailyForecast",
                table: "WeatherSettings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
