using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Golinks.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkMaxUsage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxUsage",
                table: "Links",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxUsage",
                table: "Links");
        }
    }
}
