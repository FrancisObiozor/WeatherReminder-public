using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class AddedCellVerification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCellVerified",
                table: "WeatherSettings",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCellVerified",
                table: "WeatherSettings");
        }
    }
}
