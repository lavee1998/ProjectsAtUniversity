using Microsoft.EntityFrameworkCore;
using WebShopApplication.Models;

namespace WebShopApplication.Data
{
    public class MvcWebShopContext : DbContext
    {
        public MvcWebShopContext(DbContextOptions<MvcWebShopContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Colleague> Colleagues { get; set; }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }
    }
}