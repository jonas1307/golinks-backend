using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golinks.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricDeviceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeviceModel",
                table: "Metrics",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeviceModel",
                table: "Metrics");
        }
    }
}
