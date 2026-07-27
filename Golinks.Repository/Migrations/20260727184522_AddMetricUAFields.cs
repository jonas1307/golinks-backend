using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golinks.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricUAFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Browser",
                table: "Metrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceType",
                table: "Metrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBot",
                table: "Metrics",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Os",
                table: "Metrics",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Browser",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "DeviceType",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "IsBot",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "Os",
                table: "Metrics");
        }
    }
}
