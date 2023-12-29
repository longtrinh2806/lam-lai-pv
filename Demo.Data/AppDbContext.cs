using Demo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Demo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public AppDbContext()
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=demo;user id=sa; password=yourStrong(!)Password");

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderDetail>()
                .HasKey(pc => new { pc.ProductId, pc.OrderId });

            modelBuilder.Entity<OrderDetail>()
                .HasOne(p => p.Product)
                .WithMany(pc => pc.OrderDetails)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(p => p.Order)
                .WithMany(pc => pc.OrderDetails)
                .HasForeignKey(p => p.OrderId);

            modelBuilder.Entity<Product>().Property(p => p.RowVersion).IsConcurrencyToken();

        }
        public void InitializeData(List<Product> ProductList)
        {
            foreach (var Product in ProductList)
            {
                this.Products.Add(Product);
            }
            this.SaveChanges();
        }

    }
}
