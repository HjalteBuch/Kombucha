namespace Kombucha.Models;

public class BottleReview {
    public long Id { get; set; }
    public double FizzLevel { get; set; }
    public double FunkLevel { get; set; }
    public double TasteLevel { get; set; }
    public double OverAllRating { get; set; }
    public string? Description { get; set; }

    public long BottleId { get; set; }
    public virtual Bottle Bottle { get; set; }
}

public class BottleReviewDTO {
    public long BottleId { get; set; }

    public int FizzLevel { get; set; }
    public int FunkLevel { get; set; }
    public int TasteLevel { get; set; }
    public int OverAllRating { get; set; }
    public string? Description { get; set; }
}
