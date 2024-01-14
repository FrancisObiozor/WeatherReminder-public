using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations.UnconfirmedUserDb
{
    public partial class UnconfirmedAccountsUpdate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimezoneDifference",
                table: "UnconfirmedUsers");

            migrationBuilder.AddColumn<int>(
                name: "ResendVerificationAttempts",
                table: "UnconfirmedUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResendVerificationAttempts",
                table: "UnconfirmedUsers");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimezoneDifference",
                table: "UnconfirmedUsers",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
