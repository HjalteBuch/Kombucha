using Microsoft.EntityFrameworkCore;

namespace Kombucha.Models;

public class KombuchaContext : DbContext {

    public KombuchaContext(DbContextOptions<KombuchaContext> options) : base(options){
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "Kombucha.db");
        Console.WriteLine(DbPath);
    }

    public DbSet<Batch> Batches { get; set; } = null!;
    public DbSet<Bottle> Bottles { get; set; } = null!;
    public DbSet<BottleRating> BottleRatings { get; set; } = null!;
    public DbSet<Ingredient> Ingredients { get; set; } = null!;
    public DbSet<Sugar> Sugars { get; set; } = null!;
    public DbSet<Tea> Teas { get; set; } = null!;

    public string DbPath { get; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite($"Data Source={DbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Bottle>()
            .HasOne(b => b.Batch);
        modelBuilder.Entity<BottleRating>()
            .HasOne(br => br.Bottle)
            .WithMany()
            .HasForeignKey(br => br.BottleId);
    }
}
