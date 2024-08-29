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

    [HttpGet("{id}")]
    public async Task<ActionResult<Bottle>> GetBottle(long id)
    {
        var bottle = await _context.Bottles.FindAsync(id);

        if (bottle == null) {
            return NotFound("Could not find bottle with given ID.");
        }

        return bottle;
    }

    [HttpPut("UpdateFermentationDays/{id}")]
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

        return Ok();
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
        return BadRequest("Missing to implement the new ingredients");
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
            //Ingredients = await GetIngredientsFromDTO(bottleDto.Ingredients),
            Description = bottleDto.Description,
            BatchId = bottleDto.BatchId,
        };

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
