using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly KombuchaContext _context;
    private readonly ILogger<IngredientsController> _logger;

    public IngredientsController(KombuchaContext context, ILogger<IngredientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
    {
        return await _context.Ingredients.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ingredient>> GetIngredient(long id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);

        if (ingredient == null) {
            return NotFound();
        }

        return ingredient;
    }

    [HttpPost]
    public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetIngredients), new { id = ingredient.Id }, ingredient);
    }
}
