using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class BottleRatingController : ControllerBase
{
    private readonly KombuchaContext _context;
    private readonly ILogger<BottleRatingController> _logger;

    public BottleRatingController(KombuchaContext context, ILogger<BottleRatingController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BottleRating>>> GetBottleRatings()
    {
        return await _context.BottleRatings.ToListAsync();
    }

    [HttpGet("{bottleId}")]
    public async Task<ActionResult<IEnumerable<BottleRating>>> GetBottleRatings(long bottleId)
    {
        return await _context.BottleRatings
            .Include(br => br.Bottle)
            .Where(br => br.BottleId == bottleId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> PostBottleRating([FromBody]BottleRatingDTO bottleRatingDTO) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }

        var bottleExists = await _context.Bottles.AnyAsync(b => b.Id == bottleRatingDTO.BottleId);
        if (!bottleExists) {
            ModelState.AddModelError("BottleId", "No bottle has the id provided as BottleId");
            return BadRequest(ModelState);
        }

        var bottleRating = new BottleRating {
            FizzLevel = bottleRatingDTO.FizzLevel,
            FunkLevel = bottleRatingDTO.FunkLevel,
            TasteLevel = bottleRatingDTO.TasteLevel,
            Description = bottleRatingDTO.Description,
            OverAllRating = bottleRatingDTO.OverAllRating,
            BottleId = bottleRatingDTO.BottleId
        };

        _context.BottleRatings.Add(bottleRating);
        await _context.SaveChangesAsync();


        return Ok(new { Message = "Bottle rating is submitted" });
    }
}
