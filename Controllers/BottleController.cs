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
        return await _context.Bottles.Include(b => b.Batch).Include(b => b.BottleIngredients).ThenInclude(bi => bi.Ingredient).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bottle>> GetBottle(long id)
    {
        var bottle = await _context.Bottles.Include(b => b.BottleIngredients).ThenInclude(bi => bi.Ingredient).Where(b => b.Id == id).FirstOrDefaultAsync();

        if (bottle == null) {
            return NotFound("Could not find bottle with given ID.");
        }

        return bottle;
    }

    [HttpGet("ByBatchId/{id}")]
    public async Task<ActionResult<IEnumerable<Bottle>>> GetBottlesFromBatchId(long id)
    {
        if (!_context.Batches.Any(b => b.Id == id)) {
            string errMsg = "No Batch with the given Batch ID.";
            _logger.LogError(errMsg);
            return BadRequest(errMsg);
        }

        var bottles = await _context.Bottles.Include(bo => bo.BottleIngredients).ThenInclude(bi => bi.Ingredient).Where(b => b.BatchId == id).ToListAsync();
        if (!bottles.Any()) {
            string errMsg = "No bottles from the given Batch ID.";
            _logger.LogInformation(errMsg);
            return NotFound(errMsg);
        }
        return bottles;
    }

    [HttpGet("NewestWithBottleName/{bottleName}")]
    public async Task<ActionResult<Bottle>> GetNewestBottleWithName(string bottleName)
    {
        var bottle = await _context.Bottles.Where(b => b.PhysicalBottleName == bottleName).OrderBy(b => b.TapDate).Include(bo => bo.BottleIngredients).ThenInclude(bi => bi.Ingredient).FirstOrDefaultAsync();
        if (bottle == null) {
            string errMsg = "No bottles with the bottle name " + bottleName;
            _logger.LogInformation(errMsg);
            return NotFound(errMsg);
        }
        return bottle;
    }

    [HttpPut("{id}/UpdateFermentationDays")]
    public async Task<ActionResult> UpdateFermentationDays(long id, [FromBody] BottleFermentationDTO dto)
    {
        var bottle = await _context.Bottles.FindAsync(id);

        if (bottle == null) {
            return NotFound("Could not find bottle with given ID.");
        }

        int fermentationDays = dto.EndOfFermentation.DayNumber - bottle.TapDate.DayNumber;
        if (fermentationDays <= 0) {
            return BadRequest("Date can not be the same or earlier date than the TapDate.");
        }

        bottle.DaysOfFermentation = fermentationDays;
        await _context.SaveChangesAsync();

        return Ok("Fermentation days have been updated on this bottle entry.");
    }

    [HttpPut("{id}/UpdatePhysicalBottleName/{name}")]
    public async Task<ActionResult> UpdateFermentationDays(long id, string name)
    {
        var bottle = await _context.Bottles.FindAsync(id);

        if (bottle == null) {
            return NotFound("Could not find bottle with given ID.");
        }

        bottle.PhysicalBottleName = name.ToLower();
        await _context.SaveChangesAsync();

        return Ok("The name of the physical bottle has been updated on this bottle entry.");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBottle(long id) {
        var bottle = await _context.Bottles.FindAsync(id);
        if (bottle == null) {
            return NotFound("Could not find bottle with given ID.");
        }
        _context.Remove(bottle);
        await _context.SaveChangesAsync();

        return NoContent();
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

        Bottle bottle = new Bottle {
            TapDate = bottleDto.TapDate,
            DaysOfFermentation = GetDaysOfFermentationFromDTO(bottleDto.DaysOfFermentation),
            Description = bottleDto.Description,
            BatchId = bottleDto.BatchId,
        };

        List<BottleIngredientDTO> bottleIngredients = bottleDto.BottleIngredients;
        for (int i = 0; i < bottleIngredients.Count(); i++)
        {
            BottleIngredient bottleIngredient = new BottleIngredient {
                BottleId = bottle.Id,
                IngredientId = bottleIngredients[i].Ingredient.Id,
                Grams = bottleIngredients[i].Grams
            };
            bottle.BottleIngredients.Add(bottleIngredient);
        }

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
