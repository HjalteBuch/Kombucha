using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class TeaController : ControllerBase
{
    private readonly KombuchaContext _context;
    private readonly ILogger<TeaController> _logger;

    public TeaController(KombuchaContext context, ILogger<TeaController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Tea>>> GetTeas()
    {
        return await _context.Teas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Tea>> GetTea(long id)
    {
        var tea = await _context.Teas.FindAsync(id);

        if (tea == null) {
            return NotFound();
        }

        return tea;
    }

    [HttpPost]
    public async Task<ActionResult<Tea>> PostTea(Tea tea) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        _context.Teas.Add(tea);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTea), new { id = tea.Id }, tea);
    }
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTea(long id) {
        var tea = await _context.Teas.FindAsync(id);
        if (tea == null) {
            return NotFound("Could not find tea with given ID.");
        }
        _context.Remove(tea);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
