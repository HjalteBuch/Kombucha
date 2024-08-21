namespace Kombucha.Models;

public class Ingredient {
    public long Id { get; set; }
    public string Name { get; set; }
}

public class IngredientDTO {
    public IngredientsObjectDTO[] Ingredients { get; set; }
}

public class IngredientsObjectDTO
{
    public string Name { get; set; }
    public bool Checked { get; set; }
}
