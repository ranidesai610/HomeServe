using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeServiceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAvailabilityToWeeklySchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableDate",
                table: "ProviderAvailabilities");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "ProviderAvailabilities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "ProviderAvailabilities");

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableDate",
                table: "ProviderAvailabilities",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
