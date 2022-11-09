using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Services.FavouriteMovies;
using Npgsql;

namespace MovieAppAPI.Controllers; 

[ApiController]
[Route("/api/favorites")]
[Authorize("TokenNotRejected")]
public class FavoriteMovieController : ControllerExtractToken {
    private readonly IFavoriteMovieService _favoriteMovieService;
    private readonly ILogger<FavoriteMovieController> _logger;

    public FavoriteMovieController(IFavoriteMovieService favoriteMovieService, ILogger<FavoriteMovieController> logger) {
        _favoriteMovieService = favoriteMovieService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<MovieElementModel>>> GetAll() {
        try {
            var userId = GetUserIdFromToken();
            var movies = await _favoriteMovieService.GetAll(userId);
            return Ok(movies);
        }
        catch (InvalidTokenException e) {
            _logger.LogError("{E}", e.Message);
            return BadRequest();
        }
    }

    [HttpPost("{movieId:guid}/add")]
    public async Task<IActionResult> AddMovieToFavorites(Guid movieId) {
        try {
            var userId = GetUserIdFromToken();

            await _favoriteMovieService.Create(movieId, userId);
            return Ok();
        }
        catch (AlreadyExistsException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
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

    [HttpDelete("{movieId:guid}/delete")]
    public async Task<IActionResult> DeleteMovieFromFavorites(Guid movieId) {
        try {
            var userId = GetUserIdFromToken();
            await _favoriteMovieService.Delete(movieId, userId);
            return Ok();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (InvalidTokenException e) {
            _logger.LogError("{E}", e.Message);
            return BadRequest();
        }
        catch (DbUpdateConcurrencyException e) {
            _logger.LogError("{E}", e.StackTrace);
            return BadRequest();
        }
        // TODO: вынести коды ошибок в отдельный класс
        catch (DbUpdateException e) when (e.InnerException is PostgresException { SqlState: "23505" }) {
            _logger.LogError("{E}", e.StackTrace);
            return BadRequest();
        }
    }
}