namespace Kombucha.Models;

public class Bottle {
    public long Id { get; set; }
    public DateOnly TapDate { get; set; }
    public int? DaysOfFermentation { get; set; }
    public ICollection<BottleIngredient> BottleIngredients { get; set; } = new List<BottleIngredient>();
    public string? Description { get; set; }
    public string? PhysicalBottleName { get; set; }

    public long BatchId { get; set; }
    public virtual Batch Batch { get; set; }
}

public class BottleCreateDTO {
    public DateOnly TapDate { get; set; }
    public int DaysOfFermentation { get; set; }
    public long BatchId { get; set; }
    public List<BottleIngredientDTO> BottleIngredients { get; set; }
    public string Description { get; set; }
}

public class BottleFermentationDTO {
    public DateOnly EndOfFermentation { get; set; }
}
