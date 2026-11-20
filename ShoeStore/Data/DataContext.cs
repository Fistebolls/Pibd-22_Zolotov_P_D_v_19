using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ShoeStore.Infrastructure;
using ShoeStore.Models;

namespace ShoeStore.Data;

public class DataContext : DbContext
{
    public DbSet<Manufacturer> Manufacturers => Set<Manufacturer>();

    public DbSet<Shoe> Shoes => Set<Shoe>();

    public DbSet<Receipt> Receipts => Set<Receipt>();

    public DbSet<ReceiptItem> ReceiptItems => Set<ReceiptItem>();

    public DbSet<Sale> Sales => Set<Sale>();

    public DbSet<SaleItem> SaleItems => Set<SaleItem>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = AppConfiguration.Instance.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Строка подключения DefaultConnection не найдена в appsettings.json.");

        optionsBuilder
            .UseNpgsql(connectionString)
            .LogTo(message => Serilog.Log.Information("{EntityFrameworkMessage}", message), LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureManufacturer(modelBuilder);
        ConfigureShoe(modelBuilder);
        ConfigureReceipt(modelBuilder);
        ConfigureReceiptItem(modelBuilder);
        ConfigureSale(modelBuilder);
        ConfigureSaleItem(modelBuilder);
    }

    private static void ConfigureManufacturer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.ToTable("manufacturer");
            entity.HasIndex(manufacturer => manufacturer.Name).IsUnique();
        });
    }

    private static void ConfigureShoe(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shoe>(entity =>
        {
            entity.ToTable("shoe", table => table.HasCheckConstraint("ck_shoe_price_positive", "price > 0"));
            entity.Property(shoe => shoe.Category).HasConversion<short>();
            entity.Property(shoe => shoe.Seasons).HasConversion<short>();
            entity.HasOne(shoe => shoe.Manufacturer)
                .WithMany()
                .HasForeignKey(shoe => shoe.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureReceipt(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.ToTable("receipt");
            entity.HasMany(receipt => receipt.Items)
                .WithOne(item => item.Receipt)
                .HasForeignKey(item => item.ReceiptId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(receipt => receipt.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }

    private static void ConfigureReceiptItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ReceiptItem>(entity =>
        {
            entity.ToTable("receipt_item", table => table.HasCheckConstraint("ck_receipt_item_quantity_positive", "quantity > 0"));
            entity.HasKey(item => new { item.ReceiptId, item.ShoeId, item.Size });
            entity.HasOne(item => item.Shoe)
                .WithMany()
                .HasForeignKey(item => item.ShoeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureSale(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToTable("sale");
            entity.HasMany(sale => sale.Items)
                .WithOne(item => item.Sale)
                .HasForeignKey(item => item.SaleId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Navigation(sale => sale.Items).UsePropertyAccessMode(PropertyAccessMode.Field);
        });
    }

    private static void ConfigureSaleItem(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SaleItem>(entity =>
        {
            entity.ToTable("sale_item", table =>
            {
                table.HasCheckConstraint("ck_sale_item_quantity_positive", "quantity > 0");
                table.HasCheckConstraint("ck_sale_item_price_positive", "sale_price > 0");
            });
            entity.HasKey(item => new { item.SaleId, item.ShoeId, item.Size });
            entity.HasOne(item => item.Shoe)
                .WithMany()
                .HasForeignKey(item => item.ShoeId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
