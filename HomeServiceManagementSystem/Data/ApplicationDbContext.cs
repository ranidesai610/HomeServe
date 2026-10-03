using HomeServiceManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using ServiceProviderModel = HomeServiceManagementSystem.Models.ServiceProvider;
namespace HomeServiceManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Tables
        public DbSet<User> Users { get; set; }

        public DbSet<ServiceCategory> ServiceCategories { get; set; }

        public DbSet<Service> Services { get; set; }

        public DbSet<ServiceProviderModel> ServiceProviders { get; set; }

        public DbSet<ProviderService> ProviderServices { get; set; }

        public DbSet<ProviderAvailability> ProviderAvailabilities { get; set; }

        public DbSet<Address> Addresses { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<Invoice> Invoices { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<Complaint> Complaints { get; set; }

        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // ================================
            // DECIMAL PRECISION
            // ================================

            modelBuilder.Entity<Service>()
                .Property(s => s.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Booking>()
                .Property(b => b.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Amount)
                .HasPrecision(18, 2);


            // ================================
            // USER → SERVICE PROVIDER
            // ONE USER = ONE PROVIDER PROFILE
            // ================================

            modelBuilder.Entity<ServiceProviderModel>()
                .HasOne(sp => sp.User)
                .WithOne()
                .HasForeignKey<ServiceProviderModel>(sp => sp.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ================================
            // CATEGORY → SERVICES
            // ONE CATEGORY = MANY SERVICES
            // ================================

            modelBuilder.Entity<Service>()
                .HasOne(s => s.ServiceCategory)
                .WithMany(c => c.Services)
                .HasForeignKey(s => s.ServiceCategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // PROVIDER ↔ SERVICE
            // MANY-TO-MANY THROUGH ProviderService
            // ================================

            modelBuilder.Entity<ProviderService>()
                .HasOne(ps => ps.ServiceProvider)
                .WithMany(sp => sp.ProviderServices)
                .HasForeignKey(ps => ps.ServiceProviderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProviderService>()
                .HasOne(ps => ps.Service)
                .WithMany(s => s.ProviderServices)
                .HasForeignKey(ps => ps.ServiceId)
                .OnDelete(DeleteBehavior.Cascade);


            // ================================
            // PROVIDER → AVAILABILITY
            // ONE PROVIDER = MANY AVAILABILITY
            // ================================

            modelBuilder.Entity<ProviderAvailability>()
                .HasOne(pa => pa.ServiceProvider)
                .WithMany(sp => sp.ProviderAvailabilities)
                .HasForeignKey(pa => pa.ServiceProviderId)
                .OnDelete(DeleteBehavior.Cascade);


            // ================================
            // USER → ADDRESSES
            // ONE USER = MANY ADDRESSES
            // ================================

            modelBuilder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // ================================
            // USER → BOOKINGS
            // ================================

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // SERVICE → BOOKINGS
            // ================================

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Service)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.ServiceId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // PROVIDER → BOOKINGS
            // ================================

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.ServiceProvider)
                .WithMany(sp => sp.Bookings)
                .HasForeignKey(b => b.ServiceProviderId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // ADDRESS → BOOKINGS
            // ================================

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Address)
                .WithMany(a => a.Bookings)
                .HasForeignKey(b => b.AddressId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // BOOKING → PAYMENT
            // ONE BOOKING = ONE PAYMENT
            // ================================

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Booking)
                .WithOne(b => b.Payment)
                .HasForeignKey<Payment>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            // ================================
            // BOOKING → INVOICE
            // ONE BOOKING = ONE INVOICE
            // ================================

            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Booking)
                .WithOne(b => b.Invoice)
                .HasForeignKey<Invoice>(i => i.BookingId)
                .OnDelete(DeleteBehavior.Cascade);


            // ================================
            // USER → REVIEWS
            // ================================

            modelBuilder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // BOOKING → REVIEW
            // ONE BOOKING = ONE REVIEW
            // ================================

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Booking)
                .WithOne(b => b.Review)
                .HasForeignKey<Review>(r => r.BookingId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // USER → COMPLAINTS
            // ================================

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.User)
                .WithMany(u => u.Complaints)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // BOOKING → COMPLAINTS
            // ================================

            modelBuilder.Entity<Complaint>()
                .HasOne(c => c.Booking)
                .WithMany(b => b.Complaints)
                .HasForeignKey(c => c.BookingId)
                .OnDelete(DeleteBehavior.Restrict);


            // ================================
            // USER → NOTIFICATIONS
            // ================================

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notifications)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}