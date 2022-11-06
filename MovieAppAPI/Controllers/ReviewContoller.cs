using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Reviews;
using MovieAppAPI.Services.Reviews;
using Npgsql;

namespace MovieAppAPI.Controllers;

[Route("api/movie")]
[ApiController]
[Authorize("TokenNotRejected")]
public class ReviewController : ControllerBase {
    private readonly MovieDataContext _context;
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(MovieDataContext context, IReviewService reviewService, ILogger<ReviewController> logger) {
        _context = context;
        _reviewService = reviewService;
        _logger = logger;
    }

    private string? GetUserIdFromToken() {
        var id = User.Claims.FirstOrDefault(claim =>
                claim.Type == ClaimTypes.NameIdentifier)
            ?.Value;
        return id;
    }
    
    [HttpGet("/api/review")]
    public async Task<ActionResult<IEnumerable<ReviewModel>>> GetReviews() {
        var reviews = await _reviewService.GetAll();

        if (reviews is null) {
            _logger.LogInformation("Reviews not found");
            return NotFound();
        }
        
        _logger.LogInformation("Reviews fetched");
        return Ok(reviews);
    }
    
    
    [HttpGet("/api/review/{id:guid}")]
    public async Task<ActionResult<ReviewModel>> GetReview(Guid id) {
        var review = await _reviewService.GetById(id);
    
        if (review == null) {
            _logger.LogInformation("Review with id '{Id}' not found", id.ToString());
            return NotFound($"Review with id '{id.ToString()}' not found");
        }
    
        _logger.LogInformation("Review: {@Review}", review);
        return Ok(review);
    }
    //
    // // PUT: api/ReviewContoller/5
    // // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // [HttpPut("{id:guid}")]
    // public async Task<IActionResult> PutReview(Guid id, Review review) {
    //     if (id != review.Id) {
    //         return BadRequest();
    //     }
    //
    //     _context.Entry(review).State = EntityState.Modified;
    //
    //     try {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException) {
    //         if (!ReviewExists(id)) {
    //             return NotFound();
    //         }
    //         else {
    //             throw;
    //         }
    //     }
    //
    //     return NoContent();
    // }
    
    [HttpPost("{movieId:guid}/review/add")]
    [Authorize("TokenNotRejected")]
    public async Task<ActionResult<ReviewModel>> CreateReview(Guid movieId, ReviewCreateModel reviewCreateModel) {
        try {
            var userId = GetUserIdFromToken();

            if (userId is null) {
                throw ExceptionHelper.InvalidTokenException("Token has invalid claim type");
            }

            var review = await _reviewService.Create(movieId.ToString(), userId, reviewCreateModel);
            return Created($"~api/review/{review.Id.ToString()}", review);
        }
        catch (InvalidTokenException e) {
            _logger.LogError("{E}", e.Message);
            return BadRequest();
        }
        catch (DbUpdateConcurrencyException e) {
            _logger.LogError("{E}", e.StackTrace);
            return BadRequest();
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException { SqlState: "23505" }) {
            _logger.LogError("{E}", e.StackTrace);
            return BadRequest();
        }
    }
    //
    // // DELETE: api/ReviewContoller/5
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> DeleteReview(Guid id) {
    //     if (_context.Reviews == null) {
    //         return NotFound();
    //     }
    //
    //     var review = await _context.Reviews.FindAsync(id);
    //     if (review == null) {
    //         return NotFound();
    //     }
    //
    //     _context.Reviews.Remove(review);
    //     await _context.SaveChangesAsync();
    //
    //     return NoContent();
    // }
    //
    // private bool ReviewExists(Guid id) {
    //     return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
    // }
}