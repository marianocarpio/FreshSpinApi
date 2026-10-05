using Microsoft.EntityFrameworkCore;
using FreshSpinApi.Models;

namespace FreshSpinApi.Data
{
    public class FreshSpinDbContext : DbContext
    {
        public FreshSpinDbContext(DbContextOptions<FreshSpinDbContext> options) : base(options) { }

        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<AuditLog> AuditLogs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    Id = 1,
                    CustomerId = "CUST-101",
                    ServiceType = "Wash & Fold",
                    WeightKg = 5.0,
                    TotalCost = 250.00m,
                    Status = BookingStatus.Pending,
                    PickupAddress = "Block 12 Lot 4, Manila",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}