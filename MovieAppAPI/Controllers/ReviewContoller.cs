using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;

namespace MovieAppAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewController : ControllerBase {
    private readonly MovieDataContext _context;

    public ReviewController(MovieDataContext context) {
        _context = context;
    }

    // GET: api/ReviewContoller
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Review>>> GetReviews() {
        if (_context.Reviews == null) {
            return NotFound();
        }

        return await _context.Reviews.ToListAsync();
    }

    // GET: api/ReviewContoller/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Review>> GetReview(Guid id) {
        if (_context.Reviews == null) {
            return NotFound();
        }

        var review = await _context.Reviews.FindAsync(id);

        if (review == null) {
            return NotFound();
        }

        return review;
    }

    // PUT: api/ReviewContoller/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutReview(Guid id, Review review) {
        if (id != review.Id) {
            return BadRequest();
        }

        _context.Entry(review).State = EntityState.Modified;

        try {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) {
            if (!ReviewExists(id)) {
                return NotFound();
            }
            else {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/ReviewContoller
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Review>> PostReview(Review review) {
        if (_context.Reviews == null) {
            return Problem("Entity set 'MovieDataContext.Reviews'  is null.");
        }

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetReview", new { id = review.Id }, review);
    }

    // DELETE: api/ReviewContoller/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReview(Guid id) {
        if (_context.Reviews == null) {
            return NotFound();
        }

        var review = await _context.Reviews.FindAsync(id);
        if (review == null) {
            return NotFound();
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ReviewExists(Guid id) {
        return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}