using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class registrationupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "UserLocation");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "UserLocation",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "PullLocationFromIp",
                table: "UserLocation",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "UserLocation");

            migrationBuilder.DropColumn(
                name: "PullLocationFromIp",
                table: "UserLocation");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "UserLocation",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
