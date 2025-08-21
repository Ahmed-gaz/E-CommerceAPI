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
        public DbSet<Orderr> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItems> cartItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<Cart>().Property(c => c.Id).IsRequired();
            modelBuilder.Entity<Orderr>().Property(o => o.Id).IsRequired();
            modelBuilder.Entity<Payment>().Property(p => p.Id).IsRequired();
            modelBuilder.Entity<User>().Property(u => u.Id).IsRequired();
            modelBuilder.Entity<CartItems>().Property(c => c.Id).IsRequired();
            modelBuilder.Entity<OrderItem>().Property(o => o.Id).IsRequired();


            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(u => u.User)
                .HasForeignKey<Cart>(c => c.UserId)
                .IsRequired();

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithOne(p => p.Payment)
                .HasForeignKey<Orderr>(o => o.PaymentId);

            modelBuilder.Entity<Orderr>()
                .HasOne(o => o.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(o => o.UserId)
                .IsRequired();

            modelBuilder.Entity<CartItems>()
                .HasOne(ci => ci.Cart)
                .WithMany(ci => ci.CartItems)
                .HasForeignKey(ci => ci.CartId)
                .IsRequired(); 

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(o => o.OrderId)
                .IsRequired();

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId)
                .IsRequired();



            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .IsRequired();

            modelBuilder.Entity<CartItems>()
                .Property(ci => ci.Quantity)
                .IsRequired();

            //modelBuilder.Entity<Orderr>()
            //    .Property(o => o.Payment)
            //    .
        }
    }
}
