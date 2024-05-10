using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProduct> UserProducts { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(a => a.AddressId);

                entity.Property(a => a.City).IsRequired();
                entity.Property(a => a.PostalCode).IsRequired();
                entity.Property(a => a.StreetName).IsRequired();

                entity.HasOne(a => a.User).WithMany(u => u.Addresses).HasForeignKey(a => a.UserId);

            });

            modelBuilder.Entity<Product>(entity => {
                entity.HasKey(p => p.ProductId);

                entity.Property(p => p.Name).IsRequired();
                entity.Property(p => p.Price).IsRequired();
                entity.Property(p => p.ProductCategoryId).IsRequired();

                entity.HasOne(p => p.ProductCategory).WithMany(p => p.Products).HasForeignKey(p => p.ProductCategoryId);
            });

            modelBuilder.Entity<ProductCategory>(entity =>
            {
                entity.HasKey(p=>p.CategoryId);

            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.OrderId);

             
                entity.Property(O => O.OrderDate).IsRequired();
                entity.Property(O => O.UserId).IsRequired();

                entity.HasOne(o => o.User).WithMany(u => u.Orders).HasForeignKey(o => o.UserId);
            });

            modelBuilder.Entity<OrderProduct>(entity =>
            {
                entity.HasKey(op => new { op.OrderId, op.ProductId });

                entity.HasOne(op => op.Product).WithMany(p => p.OrderProduct).HasForeignKey(op => op.ProductId);
                entity.HasOne(op => op.Order).WithMany(p => p.OrderProduct).HasForeignKey(op => op.OrderId);

            });

            modelBuilder.Entity<UserProduct>(entity =>
            {
                entity.HasKey(up=>new {up.userId,up.ProductId});

                entity.HasOne(up => up.Product).WithMany(p => p.UserProducts).HasForeignKey(up => up.ProductId);
                entity.HasOne(up => up.User).WithMany(u => u.UserProducts).HasForeignKey(up => up.userId);
            });

            


        }



    }
}
