using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QrCodeGenerator.Migrations
{
    /// <inheritdoc />
    public partial class EmailPhone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DataType",
                table: "Qrs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Qrs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Qrs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataType",
                table: "Qrs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Qrs");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Qrs");
        }
    }
}
