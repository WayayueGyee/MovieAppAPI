using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Reviews;
using MovieAppAPI.Services.Reviews;
using Npgsql;

namespace MovieAppAPI.Controllers;

[Route("api/movie")]
[ApiController]
[Authorize("TokenNotRejected")]
public class ReviewController : ControllerExtractToken {
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;
    
    public ReviewController(IReviewService reviewService, ILogger<ReviewController> logger) {
        _reviewService = reviewService;
        _logger = logger;
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


    [HttpPost("{movieId:guid}/review/add")]
    [Authorize("TokenNotRejected")]
    public async Task<ActionResult<ReviewModel>> CreateReview(Guid movieId, ReviewCreateModel reviewCreateModel) {
        try {
            var userId = GetUserIdFromToken();
            var review = await _reviewService.Create(movieId.ToString(), userId.ToString(), reviewCreateModel);
            return Created($"~api/review/{review.Id.ToString()}", review);
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (InvalidTokenException e) {
            _logger.LogError("{E}", e.Message);
            return Unauthorized();
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

    [HttpPut("/api/review/{id:guid}")]
    public async Task<IActionResult> UpdateReview(Guid id, ReviewUpdateModel reviewUpdateModel) {
        try {
            await _reviewService.Update(id, reviewUpdateModel);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (DbUpdateConcurrencyException e) {
            _logger.LogError("{E}", e.StackTrace);
            return BadRequest();
        }
    }

    [HttpPut("{movieId:guid}/review/{id:guid}/edit")]
    [Authorize]
    public async Task<IActionResult> UpdateReviewByMovieIdAndReviewId(Guid movieId, Guid id,
        ReviewUpdateModel reviewUpdateModel) {
        try {
            var userId = GetUserIdFromToken();
            await _reviewService.Update(movieId, id, userId, reviewUpdateModel);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (DbUpdateConcurrencyException e) {
            _logger.LogError("{E}", e.StackTrace);
            return BadRequest();
        }
    }

    [HttpDelete("/api/review/{id:guid}")]
    public async Task<IActionResult> DeleteReview(Guid id) {
        try {
            await _reviewService.Delete(id);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{movieId:guid}/review/{id:guid}/delete")]
    public async Task<IActionResult> DeleteReviewByMovieIdAndReviewId(Guid movieId, Guid id) {
        try {
            await _reviewService.Delete(movieId, id);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (InvalidOperationException e) {
            _logger.LogError("{E}", e.StackTrace);
            return Conflict();
        }
    }
}