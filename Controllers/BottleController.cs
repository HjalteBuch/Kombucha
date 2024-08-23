using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class BottleController : ControllerBase {
    private readonly KombuchaContext _context;
    private readonly ILogger<BottleController> _logger;

    public BottleController(KombuchaContext context, ILogger<BottleController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bottle>>> GetBottle()
    {
        return await _context.Bottles.Include(b => b.Batch).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bottle>> GetBottle(long id)
    {
        var bottle = await _context.Bottles.FindAsync(id);

        if (bottle == null) {
            return NotFound();
        }

        return bottle;
    }

    [HttpPost]
    public async Task<ActionResult<Bottle>> PostBottle([FromBody] BottleCreateDTO bottleDto) {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var batchExists = await _context.Batches.AnyAsync(b => b.Id == bottleDto.BatchId);
        if (!batchExists) {
            ModelState.AddModelError("BatchId", "No Batch has the id provided as BatchId");
            return BadRequest(ModelState);
        }

        var bottle = new Bottle {
            TapDate = bottleDto.TapDate,
            DaysOfFermentation = GetDaysOfFermentationFromDTO(bottleDto.DaysOfFermentation),
            Ingredients = await GetIngredientsFromDTO(bottleDto.Ingredients),
            Description = bottleDto.Description,
            BatchId = bottleDto.BatchId,
        };

        Console.WriteLine("bottle: ");
        Console.WriteLine(JsonSerializer.Serialize(bottle));

        _context.Bottles.Add(bottle);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBottle), new { id = bottle.Id }, bottle);
    }

    private async Task<List<Ingredient>> GetIngredientsFromDTO(IngredientIncommingDTO[] dtoIngredients)
    {
        List<long> selectedIngredientIds = dtoIngredients
            .Where(dto => dto.Checked)
            .Select(dto => dto.Value)
            .ToList();

        return await _context.Ingredients
            .Where(ingredient => selectedIngredientIds.Contains(ingredient.Id))
            .ToListAsync();
    }

    private int? GetDaysOfFermentationFromDTO(int days){
        if (days == 0) { return null; }
        return days;
    }
}
