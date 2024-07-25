namespace Kombucha.Models;

public class Bottle {
    public long Id { get; set; }
    public DateOnly TapDate { get; set; }
    public int? DaysOfFermentation { get; set; }
    public Ingredient[] Ingredients { get; set; }
    public string? Description { get; set; }

    public long BatchId { get; set; }
    public virtual Batch Batch { get; set; }
}

public class BottleCreateDTO {
    public DateOnly TapDate { get; set; }
    public Ingredient[] Ingredients { get; set; }
    public long BatchId { get; set; }
}
