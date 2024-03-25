using BarsantiExplorer.Models.Entities;
using EntityFramework.Triggers;
using Microsoft.EntityFrameworkCore;

namespace BarsantiExplorer.Models
{
    public class BarsantiDbContext : DbContext
    {
        public DbSet<Entities.Comment> Comments { get; set; }
        public DbSet<Entities.Trip> Trips { get; set; }
        public DbSet<Entities.TripType> TripTypes { get; set; }
        public DbSet<Entities.User> Users { get; set; }
        public BarsantiDbContext(DbContextOptions<BarsantiDbContext> options) : base(options)
        {

        }
    }
}
