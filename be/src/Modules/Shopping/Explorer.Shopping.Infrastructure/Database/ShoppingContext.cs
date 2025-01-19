using Explorer.Shopping.Core.Domain;
using Explorer.Shopping.Core.Domain.ShoppingCarts;
using Microsoft.EntityFrameworkCore;

namespace Explorer.Shopping.Infrastructure.Database;

public class ShoppingContext : DbContext
{
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<TourPurchaseToken> PurchaseTokens { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Coupon> Coupons { get; set; }
    public DbSet<TouristWallet> TouristWallets { get; set; }
    public DbSet<Bundle> Bundles { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<PaymentRecord> PaymentRecords { get; set; }
    public ShoppingContext(DbContextOptions<ShoppingContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("shopping");
        modelBuilder.Entity<ShoppingCart>()
           .Property(cart => cart.Items)
           .HasColumnType("jsonb");
        ConfigureShoppingCart(modelBuilder);

        modelBuilder.Entity<Bundle>()
            .HasMany(b => b.Products)
            .WithOne();
    }
    
    private static void ConfigureShoppingCart(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TourPurchaseToken>()
            .HasIndex(tpt => new { tpt.UserId, tpt.TourId })
            .IsUnique();
    }
}
