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
    public async Task<ActionResult<IEnumerable<BatchDTO>>> GetBatches()
    {
        //var batches = await _context.Batches.ToListAsync();
        var batches = from b in _context.Batches
            select new BatchDTO() {
                Id = b.Id,
                StartTime = b.StartTime,
                SugarType = b.Sugar.Name,
                GramsOfSugar = b.GramsOfSugar,
                TeaType = b.Tea.Name + ", " + b.Tea.Brand,
                GramsOfTea = b.GramsOfTea,
                SteepMinutes = b.SteepMinutes,
                Description = b.Description,
            };
        return await batches.ToListAsync();
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
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        _context.Batches.Add(batch);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBatch), new { id = batch.Id }, batch);
    }
}
