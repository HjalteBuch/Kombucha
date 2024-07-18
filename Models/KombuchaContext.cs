using Microsoft.EntityFrameworkCore;

namespace Kombucha.Models;

public class KombuchaContext : DbContext {
    public KombuchaContext(DbContextOptions<KombuchaContext> options) : base(options){
    }

    public DbSet<Batch> Batches { get; set; } = null!;
    public DbSet<Bottle> Bottles { get; set; } = null!;
    public DbSet<BottleRating> BottleRatings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Bottle>()
            .HasOne(b => b.Batch);
        modelBuilder.Entity<BottleRating>()
            .HasOne(br => br.Bottle)
            .WithMany()
            .HasForeignKey(br => br.BottleId);
    }
}
