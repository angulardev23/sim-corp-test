using Microsoft.EntityFrameworkCore;

namespace BackendTest.Infrastructure.Persistence
{
    public sealed class BackendTestDbContext : DbContext
    {
        public BackendTestDbContext(DbContextOptions<BackendTestDbContext> options) : base(options)
        {
        }

        internal DbSet<PersonRecord> People => Set<PersonRecord>();
        internal DbSet<ProductRecord> Products => Set<ProductRecord>();
        internal DbSet<PurchaseRecord> Purchases => Set<PurchaseRecord>();
        internal DbSet<PurchaseProductRecord> PurchaseProducts => Set<PurchaseProductRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PersonRecord>().HasKey(person => person.Id);
            modelBuilder.Entity<ProductRecord>().HasKey(product => product.Id);
            modelBuilder.Entity<PurchaseRecord>().HasKey(purchase => purchase.Id);
            modelBuilder.Entity<PurchaseRecord>()
                .HasOne(purchase => purchase.Customer)
                .WithMany()
                .HasForeignKey(purchase => purchase.CustomerId);
            modelBuilder.Entity<PurchaseProductRecord>()
                .HasKey(item => new { item.PurchaseId, item.ProductId });
            modelBuilder.Entity<PurchaseProductRecord>()
                .HasOne(item => item.Purchase)
                .WithMany(purchase => purchase.Products)
                .HasForeignKey(item => item.PurchaseId);
            modelBuilder.Entity<PurchaseProductRecord>()
                .HasOne(item => item.Product)
                .WithMany()
                .HasForeignKey(item => item.ProductId);

            modelBuilder.Entity<PersonRecord>().HasData(SeedData.People());
            modelBuilder.Entity<ProductRecord>().HasData(SeedData.Products());
            modelBuilder.Entity<PurchaseRecord>().HasData(SeedData.Purchases());
            modelBuilder.Entity<PurchaseProductRecord>().HasData(SeedData.PurchaseProducts());
        }
    }
}
