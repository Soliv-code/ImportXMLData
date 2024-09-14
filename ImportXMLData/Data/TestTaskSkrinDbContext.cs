using ImportXMLData.Domain;
using Microsoft.EntityFrameworkCore;

namespace ImportXMLData.Data
{
    public class TestTaskSkrinDbContext : DbContext
    {
        public TestTaskSkrinDbContext()
        {
        }
        public TestTaskSkrinDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPurchase> UserPurchases { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Да, я знаю что это не безопасно, но ещё ни разу в жизни не приходилось делать DbContext в консольном приложении
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=TestTaskSkrin;Trusted_Connection=False;User Id=sa;Password=12345;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPurchase>()
                .HasMany(up => up.PurchaseDetails)
                .WithOne(pd => pd.UserPurchase)
                .HasForeignKey(pd => pd.UserPurchaseId);

            modelBuilder.Entity<Product>()
                .HasMany(p => p.PurchaseDetails)
                .WithOne(pd => pd.Product)
                .HasForeignKey(pd => pd.ProductId);
        }

    }
}
