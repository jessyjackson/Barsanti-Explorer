using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarsantiExplorer.Migrations
{
    /// <inheritdoc />
    public partial class renaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvarageRating",
                table: "Trips",
                newName: "TotalRating");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalRating",
                table: "Trips",
                newName: "AvarageRating");
        }
    }
}
