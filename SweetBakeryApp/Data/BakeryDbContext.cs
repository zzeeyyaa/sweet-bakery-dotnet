using Microsoft.EntityFrameworkCore;
using SweetBakeryApp.Models;

namespace SweetBakeryApp.Data;

public class BakeryDbContext : DbContext
{

    //contructor for web
    public BakeryDbContext(DbContextOptions<BakeryDbContext> options) : base(options) { }

    //contructor default for migration tool
    public BakeryDbContext() { }

    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<OrderTicket> OrderTickets => Set<OrderTicket>();
    public DbSet<BakedProduct> BakedProducts => Set<BakedProduct>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // base.OnConfiguring(optionsBuilder);
        // string connectionString = "Server=localhost\\SQLEXPRESS;Database=SweetBakeryDb;Trusted_Connection=True;TrustServerCertificate=True;";
        // optionsBuilder.UseSqlServer(connectionString);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=SweetBakeryDb;Trusted_Connection=True;TrustServerCertificate=True;");
        }

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //butuh konfigurasi presisi desimal untuk harga
        modelBuilder.Entity<MenuItem>()
        .Property(m => m.Price)
        .HasPrecision(18, 2);
    }
}