using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golinks.Repository.Migrations
{
    /// <inheritdoc />
    public partial class LinksAddTotalUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalUsage",
                table: "Links",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalUsage",
                table: "Links");
        }
    }
}
