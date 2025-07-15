using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MXANH.Enums;
using MXANH.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PointsTransaction> PointsTransactions { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<MarketplaceProduct> MarketplaceProducts { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialDetail> MaterialDetails { get; set; }
    public DbSet<CollectionRequest> CollectionRequests { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }
    public DbSet<EnterpriseProfile> EnterpriseProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User - Address (1:N)
        modelBuilder.Entity<Address>()
            .HasOne(a => a.User)
            .WithMany(u => u.Addresses)
            .HasForeignKey(a => a.UserId);

        // User - PointsTransaction (1:N)
        modelBuilder.Entity<PointsTransaction>()
            .HasOne(pt => pt.User)
            .WithMany(u => u.PointsTransactions)
            .HasForeignKey(pt => pt.UserId);

        // User - Orders (1:N)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        // Product - Orders (1:N)
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Product)
            .WithMany(p => p.Orders)
            .HasForeignKey(o => o.ProductId);

        // User - CollectionRequest (1:N)
        modelBuilder.Entity<CollectionRequest>()
            .HasOne(cr => cr.User)
            .WithMany(u => u.CollectionRequests)
            .HasForeignKey(cr => cr.UserId);

        // Material - CollectionRequest (1:N)
        modelBuilder.Entity<CollectionRequest>()
            .HasOne(cr => cr.Material)
            .WithMany(m => m.CollectionRequests)
            .HasForeignKey(cr => cr.MaterialId);

        // UserEvent - User (N:1)
        modelBuilder.Entity<UserEvent>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserEvents)
            .HasForeignKey(ue => ue.UserId);

        // UserEvent - Event (N:1)
        modelBuilder.Entity<UserEvent>()
            .HasOne(ue => ue.Event)
            .WithMany(e => e.UserEvents)
            .HasForeignKey(ue => ue.EventId);
    }
}
