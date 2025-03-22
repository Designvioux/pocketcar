using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Cars_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Transission",
                table: "Cars",
                newName: "DriverPhoneNumber");

            migrationBuilder.RenameColumn(
                name: "RegistrationNumber",
                table: "Cars",
                newName: "DriverName");

            migrationBuilder.RenameColumn(
                name: "CarId",
                table: "Cars",
                newName: "Id");

            migrationBuilder.AddColumn<bool>(
                name: "ACAvailable",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "CarFeatures",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CarNumber",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CarPhotos",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverBloodGroup",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverEmail",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DriverLocation",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ACAvailable",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarFeatures",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarNumber",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarPhotos",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DriverBloodGroup",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DriverEmail",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "DriverLocation",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "DriverPhoneNumber",
                table: "Cars",
                newName: "Transission");

            migrationBuilder.RenameColumn(
                name: "DriverName",
                table: "Cars",
                newName: "RegistrationNumber");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cars",
                newName: "CarId");
        }
    }
}
