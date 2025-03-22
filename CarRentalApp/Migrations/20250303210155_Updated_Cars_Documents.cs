using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CarRentalApp.Migrations
{
    /// <inheritdoc />
    public partial class Updated_Cars_Documents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CarDocuments",
                table: "Cars",
                newName: "Tax");

            migrationBuilder.AddColumn<string>(
                name: "FitnessCertificate",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Insurance",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PUC",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Permit",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FitnessCertificate",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Insurance",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "PUC",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Permit",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "Tax",
                table: "Cars",
                newName: "CarDocuments");
        }
    }
}
