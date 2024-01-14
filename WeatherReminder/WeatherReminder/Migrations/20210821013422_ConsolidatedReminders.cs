using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class ConsolidatedReminders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Snow_ReminderDetails_ReminderDetailsId",
                table: "Snow");

            migrationBuilder.DropForeignKey(
                name: "FK_Temperature_ReminderDetails_ReminderDetailsId",
                table: "Temperature");

            migrationBuilder.DropForeignKey(
                name: "FK_Umbrella_ReminderDetails_ReminderDetailsId",
                table: "Umbrella");

            migrationBuilder.DropIndex(
                name: "IX_Umbrella_ReminderDetailsId",
                table: "Umbrella");

            migrationBuilder.DropIndex(
                name: "IX_Temperature_ReminderDetailsId",
                table: "Temperature");

            migrationBuilder.DropIndex(
                name: "IX_Snow_ReminderDetailsId",
                table: "Snow");

            migrationBuilder.DropColumn(
                name: "ReminderDetailsId",
                table: "Umbrella");

            migrationBuilder.DropColumn(
                name: "ReminderDetailsId",
                table: "Temperature");

            migrationBuilder.DropColumn(
                name: "ReminderDetailsId",
                table: "Snow");

            migrationBuilder.AddColumn<string>(
                name: "Reminders",
                table: "WeatherSettings",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsReminder",
                table: "Umbrella",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReminder",
                table: "Temperature",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReminder",
                table: "Snow",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reminders",
                table: "WeatherSettings");

            migrationBuilder.DropColumn(
                name: "IsReminder",
                table: "Umbrella");

            migrationBuilder.DropColumn(
                name: "IsReminder",
                table: "Temperature");

            migrationBuilder.DropColumn(
                name: "IsReminder",
                table: "Snow");

            migrationBuilder.AddColumn<int>(
                name: "ReminderDetailsId",
                table: "Umbrella",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReminderDetailsId",
                table: "Temperature",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReminderDetailsId",
                table: "Snow",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Umbrella_ReminderDetailsId",
                table: "Umbrella",
                column: "ReminderDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Temperature_ReminderDetailsId",
                table: "Temperature",
                column: "ReminderDetailsId");

            migrationBuilder.CreateIndex(
                name: "IX_Snow_ReminderDetailsId",
                table: "Snow",
                column: "ReminderDetailsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Snow_ReminderDetails_ReminderDetailsId",
                table: "Snow",
                column: "ReminderDetailsId",
                principalTable: "ReminderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Temperature_ReminderDetails_ReminderDetailsId",
                table: "Temperature",
                column: "ReminderDetailsId",
                principalTable: "ReminderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Umbrella_ReminderDetails_ReminderDetailsId",
                table: "Umbrella",
                column: "ReminderDetailsId",
                principalTable: "ReminderDetails",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
