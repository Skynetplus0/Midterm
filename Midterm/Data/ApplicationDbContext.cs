using Midterm.Api.Models;
using Midterm.Models;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;

namespace Midterm.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Listing> Listings => Set<Listing>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<GuestQueryUsage> GuestQueryUsages => Set<GuestQueryUsage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Listing -> Host
            modelBuilder.Entity<Listing>()
                .HasOne(l => l.Host)
                .WithMany(u => u.Listings)
                .HasForeignKey(l => l.HostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Listing
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Listing)
                .WithMany(l => l.Bookings)
                .HasForeignKey(b => b.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Booking -> Guest
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Guest)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review -> Booking (1 to 1)
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasIndex(r => r.BookingId)
                .IsUnique();

            // Review -> Listing
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Listing)
                .WithMany(l => l.Reviews)
                .HasForeignKey(r => r.ListingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Review -> Guest
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Guest)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // GuestQueryUsage
            modelBuilder.Entity<GuestQueryUsage>()
                .HasIndex(g => new { g.ClientKey, g.QueryDate })
                .IsUnique();

            // Decimal precision
            modelBuilder.Entity<Listing>()
                .Property(l => l.Price)
                .HasPrecision(18, 2);
        }
    }
}