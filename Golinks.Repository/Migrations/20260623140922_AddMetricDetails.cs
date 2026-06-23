using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golinks.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddMetricDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IpHash",
                table: "Metrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "Metrics",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserAgent",
                table: "Metrics",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IpHash",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "Metrics");

            migrationBuilder.DropColumn(
                name: "UserAgent",
                table: "Metrics");
        }
    }
}
