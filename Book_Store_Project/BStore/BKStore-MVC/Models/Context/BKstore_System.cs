using BKStore_MVC.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BKStore_MVC.Models.Context
{
    public class BKstore_System : IdentityDbContext<ApplicationUser>
    {
        public BKstore_System() : base()
        {

        }
        public BKstore_System(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                    .HasOne(c => c.User)
                    .WithOne(u => u.Customer)
                    .HasForeignKey<Customer>(c => c.UserID);

            modelBuilder.Entity<Reviews>()
                    .HasOne(r => r.User)
                    .WithOne(u => u.Reviews)
                    .HasForeignKey<Reviews>(r => r.UserID);

            modelBuilder.Entity<OrderBook>()
                    .HasKey(i => new { i.OrderID, i.BookID });

            modelBuilder.Entity<Customer>();

            modelBuilder.Entity<Reviews>()
                    .Property(r => r.Rating)
                    .HasPrecision(2, 1);

            modelBuilder.Entity<ReviewsDelivery>()
                    .Property(r => r.Rating)
                    .HasPrecision(2, 1);
        }
        public DbSet<Book> Book { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderBook> OrderBook { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Shipping> Shipping { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<Governorate> governorate { get; set; }
        public DbSet<DeliveryClients> DeliveryClients { get; set; }
        public DbSet<ReviewsDelivery> ReviewsDelivery { get; set; }
        public DbSet<BookRating> BookRatings { get; set; }
        public DbSet<ShippingMethod> shippingMethod { get; set; }
    }
}
