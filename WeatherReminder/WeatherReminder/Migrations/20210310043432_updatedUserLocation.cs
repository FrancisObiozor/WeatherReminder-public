using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class updatedUserLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.AddColumn<string>(
                name: "Latitude",
                table: "UserLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationKey",
                table: "UserLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Longitude",
                table: "UserLocation",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Reminders",
                table: "ReminderDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "UserLocation");

            migrationBuilder.DropColumn(
                name: "LocationKey",
                table: "UserLocation");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "UserLocation");

            migrationBuilder.DropColumn(
                name: "Reminders",
                table: "ReminderDetails");

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaysBeforeEvent = table.Column<int>(type: "int", nullable: false),
                    ReminderDetailsId = table.Column<int>(type: "int", nullable: true),
                    ReminderTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminder_ReminderDetails_ReminderDetailsId",
                        column: x => x.ReminderDetailsId,
                        principalTable: "ReminderDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_ReminderDetailsId",
                table: "Reminder",
                column: "ReminderDetailsId");
        }
    }
}
