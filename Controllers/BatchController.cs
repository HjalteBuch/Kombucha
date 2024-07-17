using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class BatchController : ControllerBase
{
    private readonly KombuchaContext _context;
    private readonly ILogger<BatchController> _logger;

    public BatchController(KombuchaContext context, ILogger<BatchController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Batch>>> GetBatches()
    {
        return await _context.Batches.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Batch>> GetBatch(long id)
    {
        var batch = await _context.Batches.FindAsync(id);

        if (batch == null) {
            return NotFound();
        }

        return batch;
    }

    [HttpPost]
    public async Task<ActionResult<Batch>> PostBatch(Batch batch) {
        _context.Batches.Add(batch);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBatch), new { id = batch.Id }, batch);
    }
}
