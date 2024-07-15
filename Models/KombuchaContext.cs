using Microsoft.EntityFrameworkCore;

namespace Kombucha.Models;

public class KombuchaContext : DbContext {
    public KombuchaContext(DbContextOptions<KombuchaContext> options) : base(options){
    }

    public DbSet<Batch> Batches { get; set; } = null!;
    public DbSet<Bottle> Bottles { get; set; } = null!;
}
