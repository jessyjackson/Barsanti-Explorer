using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarsantiExplorer.Migrations
{
    /// <inheritdoc />
    public partial class addStatusandRatingField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Comments");

            migrationBuilder.AddColumn<double>(
                name: "AvarageRating",
                table: "Trips",
                type: "double",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RatingsNumber",
                table: "Trips",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rating",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvarageRating",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "RatingsNumber",
                table: "Trips");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Comments");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Comments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
