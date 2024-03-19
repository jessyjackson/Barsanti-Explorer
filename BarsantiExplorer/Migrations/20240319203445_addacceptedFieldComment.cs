using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarsantiExplorer.Migrations
{
    /// <inheritdoc />
    public partial class addacceptedFieldComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "Comments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "Comments");
        }
    }
}
