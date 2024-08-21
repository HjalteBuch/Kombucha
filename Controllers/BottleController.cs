using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

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
            Ingredients = bottleDto.Ingredients,
            BatchId = bottleDto.BatchId
        };

        //_context.Bottles.Add(bottle);
        //await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBottle), new { id = bottle.Id }, bottle);
    }
}
