namespace SharedTrip.Data
{
    using Microsoft.EntityFrameworkCore;
    using SharedTrip.Data.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Trip> Trips { get; set; }

        public DbSet<UserTrip> UserTrips { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(DatabaseConfiguration.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTrip>(e =>
            {

                e.HasKey(k => new { k.UserId, k.TripId });

                e.HasOne(u => u.User)
                .WithMany("UserTrips")
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(t => t.Trip)
                .WithMany("UserTrips")
                .HasForeignKey(t => t.TripId)
                .OnDelete(DeleteBehavior.Restrict);

            });
        }
    }
}
