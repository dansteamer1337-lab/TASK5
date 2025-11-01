using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApplication7.Models;

namespace WebApplication7.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; } // Добавляем Products

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация Users
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Login).IsRequired().HasMaxLength(50);
            });

            // Конфигурация Products
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.IsDeleted).HasDefaultValue(false);

                // Индекс для быстрого поиска неудаленных продуктов
                entity.HasIndex(p => p.IsDeleted);
            });

            // Добавляем тестовые продукты
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "Gaming laptop", Price = 999.99m, IsDeleted = false },
                new Product { Id = 2, Name = "Mouse", Description = "Wireless mouse", Price = 29.99m, IsDeleted = false },
                new Product { Id = 3, Name = "Keyboard", Description = "Mechanical keyboard", Price = 79.99m, IsDeleted = false },
                new Product { Id = 4, Name = "Monitor", Description = "27 inch 4K monitor", Price = 399.99m, IsDeleted = false },
                new Product { Id = 5, Name = "Headphones", Description = "Noise cancelling headphones", Price = 199.99m, IsDeleted = false }
            );
        }
    }
}