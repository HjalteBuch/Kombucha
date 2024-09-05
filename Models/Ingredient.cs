using System.Text.Json.Serialization;

namespace Kombucha.Models;

public class Ingredient {
    public long Id { get; set; }
    public string Name { get; set; }

    [JsonIgnore]
    public ICollection<BottleIngredient> BottleIngredients { get; set; } = new List<BottleIngredient>();
}

public class IngredientIncommingDTO
{
    public string Name { get; set; }
    public bool Checked { get; set; }
    public long Value { get; set; }
    public int Grams { get; set; }
}
