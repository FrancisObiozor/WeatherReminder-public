using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WeatherReminder.Migrations
{
    public partial class UnconfirmedAccounts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnconfirmedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(nullable: true),
                    AccountCreated = table.Column<DateTime>(nullable: false),
                    TimezoneDifference = table.Column<TimeSpan>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnconfirmedUsers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnconfirmedUsers");
        }
    }
}
