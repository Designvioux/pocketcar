using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Trips_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PickUpDateTime",
                table: "Trips",
                newName: "PickUpDate");

            migrationBuilder.RenameColumn(
                name: "DropOffDateTime",
                table: "Trips",
                newName: "DropOffDate");

            migrationBuilder.AddColumn<string>(
                name: "CarName",
                table: "Trips",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverBloodGroup",
                table: "Trips",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverNumber",
                table: "Trips",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "DropOffTime",
                table: "Trips",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "PickUpTime",
                table: "Trips",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarName",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DriverBloodGroup",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DriverNumber",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "DropOffTime",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "PickUpTime",
                table: "Trips");

            migrationBuilder.RenameColumn(
                name: "PickUpDate",
                table: "Trips",
                newName: "PickUpDateTime");

            migrationBuilder.RenameColumn(
                name: "DropOffDate",
                table: "Trips",
                newName: "DropOffDateTime");
        }
    }
}
