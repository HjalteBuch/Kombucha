using Microsoft.AspNetCore.Mvc;
using Kombucha.Models;
using Microsoft.EntityFrameworkCore;

namespace Kombucha.Controllers;

[ApiController]
[Route("[controller]")]
public class BottleReviewController : ControllerBase
{
    private readonly KombuchaContext _context;
    private readonly ILogger<BottleReviewController> _logger;

    public BottleReviewController(KombuchaContext context, ILogger<BottleReviewController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BottleReview>>> GetBottleReviews()
    {
        return await _context.BottleReviews.ToListAsync();
    }

    [HttpGet("{bottleId}")]
    public async Task<ActionResult<IEnumerable<BottleReview>>> GetBottleReviews(long bottleId)
    {
        return await _context.BottleReviews
            .Include(br => br.Bottle)
            .Where(br => br.BottleId == bottleId)
            .ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> PostBottleReview([FromBody]BottleReviewDTO bottleReviewDTO) {
        if (!ModelState.IsValid) {
            return BadRequest(ModelState);
        }
        Console.WriteLine(bottleReviewDTO.BottleId);
        var bottleExists = await _context.Bottles.AnyAsync(b => b.Id == bottleReviewDTO.BottleId);
        if (!bottleExists) {
            ModelState.AddModelError("BottleId", "No bottle has the id provided as BottleId");
            return BadRequest(ModelState);
        }

        var bottleReview = new BottleReview {
            BottleId = bottleReviewDTO.BottleId,
            FizzLevel = bottleReviewDTO.FizzLevel,
            FunkLevel = bottleReviewDTO.FunkLevel,
            TasteLevel = bottleReviewDTO.TasteLevel,
            Description = bottleReviewDTO.Description,
            OverAllRating = bottleReviewDTO.OverAllRating
        };

        _context.BottleReviews.Add(bottleReview);
        await _context.SaveChangesAsync();


        return Ok(new { Message = "Bottle review is submitted" });
    }
}
