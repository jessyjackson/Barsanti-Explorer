using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarsantiExplorer.Migrations
{
    /// <inheritdoc />
    public partial class triggers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP TRIGGER IF EXISTS `CalculateAverage`;
                                SET @OLDTMP_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';
                                CREATE TRIGGER `CalculateAverage` AFTER INSERT ON `comments` FOR EACH ROW BEGIN
                                UPDATE trips
                                SET
                                trips.RatingsNumber = trips.RatingsNumber + 1,
                                trips.TotalRating = trips.TotalRating + NEW.Rating,
                                trips.AverageRating = trips.TotalRating / trips.RatingsNumber
                                WHERE Id = NEW.TripId;
                                END;
                                SET SQL_MODE=@OLDTMP_SQL_MODE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
