namespace Kombucha.Models;

public class Batch {
    public long Id { get; set; }
    public DateOnly StartTime { get; set; }
    public int Sugar { get; set; }
    public string Tea { get; set; }
    public string Description { get; set; }
}
