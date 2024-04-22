using Microsoft.EntityFrameworkCore;
using VendorManagementSystem.Models.Models;

namespace VendorManagementSystem.Infrastructure.Data
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options) { }
        public DbSet<User> Users { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Category> Catrogries { get; set; }
        public DbSet<VendorType> VendorTypes { get; set; }

        public DbSet<UserToken> UserTokens { get; set; }
        public  DbSet<VendorCategoryMapping> VendorCategoryMappings { get; set; }
        public object VendorCategoryMapping { get; internal set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VendorType>().HasData(
                new VendorType {Id=1, Name = "New Vendors" },
                new VendorType {Id=2, Name = "Completed Vendors" },
                new VendorType {Id=3, Name = "In Progress" },
                new VendorType {Id=4, Name = "On Hold" },
                new VendorType {Id=5, Name = "Need Follow Up" }
            );

            modelBuilder.Entity<VendorCategoryMapping>()
                .HasKey(m => new { m.VendorId, m.CategoryId });

            modelBuilder.Entity<VendorCategoryMapping>()
                .HasOne<Vendor>(m => m.Vendor)
                .WithMany(v => v.VendorCategoryMapping)
                .HasForeignKey(m => m.VendorId);

            modelBuilder.Entity<VendorCategoryMapping>()
                .HasOne<Category>(m => m.Category)
                .WithMany(c => c.VendorCategoryMapping)
                .HasForeignKey(m => m.CategoryId);

            modelBuilder.Entity<UserToken>()
                .HasKey(ut => new {ut.Email});
        }
    }
}
