using Microsoft.EntityFrameworkCore;
using CROPDEAL.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CROPDEAL.Data
{
    public class CropDealDbContext : DbContext
    {
        public CropDealDbContext(DbContextOptions options) : base(options)
        {

        }



        public DbSet<User> Users { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Crop> Crops { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.User)
                .WithMany()
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        }

    }
}