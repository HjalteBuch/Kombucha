using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class SugarController : ControllerBase
{
    private readonly KombuchaContext _context;
    private readonly ILogger<SugarController> _logger;

    public SugarController(KombuchaContext context, ILogger<SugarController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Sugar>>> GetSugars()
    {
        return await _context.Sugars.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Sugar>> GetSugar(long id)
    {
        var sugar = await _context.Sugars.FindAsync(id);

        if (sugar == null) {
            return NotFound();
        }

        return sugar;
    }

    [HttpPost]
    public async Task<ActionResult<Sugar>> PostSugar(Sugar sugar) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _context.Sugars.Add(sugar);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetSugar), new { id = sugar.Id }, sugar);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteSugar(long id) {
        var sugar = await _context.Teas.FindAsync(id);
        if (sugar == null) {
            return NotFound("Could not find sugar with given ID.");
        }
        _context.Remove(sugar);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
