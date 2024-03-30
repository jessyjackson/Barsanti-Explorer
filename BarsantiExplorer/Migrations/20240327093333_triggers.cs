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
                                CREATE TRIGGER `CalculateAverage` AFTER UPDATE ON `comments` FOR EACH ROW
                                BEGIN
                                   IF NEW.status = 1 THEN
                                       UPDATE trips
                                       SET
                                           RatingsNumber = RatingsNumber + 1,
                                           TotalRating = TotalRating + NEW.Rating,
                                           AverageRating = TotalRating / RatingsNumber
                                       WHERE Id = NEW.TripId;
                                   END IF;
                                   IF NEW.DeletedAt <> OLD.DeletedAt then
                                      UPDATE trips
                                       SET
                                           RatingsNumber = RatingsNumber - 1,
                                           TotalRating = TotalRating - NEW.Rating,
                                           AverageRating = TotalRating / RatingsNumber
                                       WHERE Id = NEW.TripId;
                                   END IF;
                                END;
                                SET SQL_MODE=@OLDTMP_SQL_MODE;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
