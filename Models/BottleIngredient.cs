namespace Kombucha.Models;
using System.Text.Json.Serialization;

public class BottleIngredient {
    public long BottleId { get; set; }
    [JsonIgnore]
    public Bottle Bottle { get; set; }

    public long IngredientId { get; set; }
    public Ingredient Ingredient { get; set; }

    public int Grams { get; set; }
}

public class BottleIngredientDTO {
    public Ingredient Ingredient { get; set; }
    public int Grams { get; set; }
}
