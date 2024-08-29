using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

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
                TeaType = b.Tea.Name,
                GramsOfTea = b.GramsOfTea,
                SteepMinutes = b.SteepMinutes,
                Description = b.Description,
            };
        return await batches.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BatchDTO>> GetBatch(long id) {
        var batch = await _context.Batches
            .Include(b => b.Sugar)
            .Include(b => b.Tea)
            .Where(b => b.Id == id)
            .FirstAsync();

        if (batch == null) {
            return NotFound();
        }

        var batchDTO = new BatchDTO {
                Id = batch.Id,
                StartTime = batch.StartTime,
                SugarType = batch.Sugar.Name,
                GramsOfSugar = batch.GramsOfSugar,
                TeaType = batch.Tea.Name,
                GramsOfTea = batch.GramsOfTea,
                SteepMinutes = batch.SteepMinutes,
                Description = batch.Description,
        };

        return batchDTO;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBatch(long id) {
        var batch = await _context.Batches.FindAsync(id);
        if (batch == null) {
            return NotFound("Batch not found with the provided id");
        }
        _context.Remove(batch);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<Batch>> PostBatch(BatchPostDTO batchPost) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        bool error = false;

        var sugarExists = await _context.Sugars.AnyAsync(s => s.Id == batchPost.SugarId);
        if (!sugarExists) {
            ModelState.AddModelError("SugarId", "No sugar has the id provided as the sugar id");
            error = true;
        }

        var TeaExists = await _context.Teas.AnyAsync(t => t.Id == batchPost.TeaId);
        if (!TeaExists) {
            ModelState.AddModelError("TeaId", "No tea has the id provided as the tea id");
            error = true;
        }

        if (error) {
            return BadRequest(ModelState);
        }

        var batch = new Batch {
            StartTime = batchPost.StartTime,
            GramsOfSugar = batchPost.GramsOfSugar,
            GramsOfTea = batchPost.GramsOfTea,
            SteepMinutes = batchPost.SteepMinutes,
            Description = batchPost.Description,
            SugarId = batchPost.SugarId,
            TeaId = batchPost.TeaId
        };

        _context.Batches.Add(batch);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBatch), new { id = batch.Id }, batch);
    }
}
