namespace Kombucha.Models;

public class BottleRating {
    public long Id { get; set; }
    public int? FizzLevel { get; set; }
    public int? FunkLevel { get; set; }
    public int? TasteLevel { get; set; }
    public string? Description { get; set; }
    public int OverAllRating { get; set; }

    public long BottleId { get; set; }
    public virtual Bottle Bottle { get; set; }
}

public class BottleRatingDTO {
    public int? FizzLevel { get; set; }
    public int? FunkLevel { get; set; }
    public int? TasteLevel { get; set; }
    public string? Description { get; set; }
    public int OverAllRating { get; set; }

    public long BottleId { get; set; }
}
