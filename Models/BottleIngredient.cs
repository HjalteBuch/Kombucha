namespace Kombucha.Models;

public class BottleIngredient {
    public long BottleId { get; set; }
    public Bottle Bottle { get; set; }

    public long IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }

    public int Grams { get; set; }
}
