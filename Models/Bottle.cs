namespace Kombucha.Models;

public class Bottle {
    public long Id { get; set; }
    public DateOnly TapDate { get; set; }
    public int DaysOfFermentation { get; set; }
    public string Ingredients { get; set; }
    public int FizzLevel { get; set; }
    public string Description { get; set; }

    public long BatchId { get; set; }
    public virtual Batch Batch { get; set; }
}
