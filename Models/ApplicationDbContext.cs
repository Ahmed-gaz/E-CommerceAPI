using Microsoft.EntityFrameworkCore;
using System;


namespace E_CommerceAPI.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Cart>().Property(c => c.Id).IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.Id).IsRequired();
            modelBuilder.Entity<Payment>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Id).IsRequired();
            modelBuilder.Entity<CartItems>().Property(c => c.Id).IsRequired();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(u => u.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .HasOne(u => u.Order)
                .WithOne(u => u.Payment)
                .HasForeignKey<Order>(c => c.PaymentId)
                .IsRequired();

            modelBuilder.Entity<Order>()
                .HasOne(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.UserId)
                .IsRequired();

            modelBuilder.Entity<CartItems>()
                .HasOne(c => c.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .IsRequired(); 
            
            //modelBuilder.Entity<CartItems>()
            //    .HasOne(p => p.Product)
            //    .WithMany(c => c.CartItems)
            //    .HasForeignKey(p => p.ProductId)
            //    .IsRequired();

            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<CartItems>()
                .Property(ci => ci.Quantity)
                .IsRequired();
        }
    }
}
