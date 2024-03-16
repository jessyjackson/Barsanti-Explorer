using BarsantiExplorer.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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
