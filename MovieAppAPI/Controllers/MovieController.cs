using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Services.Movies;

namespace MovieAppAPI.Controllers;

[Route("api/movie")]
[Produces("application/json")]
[ApiController]
public class MovieController : ControllerBase {
    private readonly IMovieService _movieService;
    private readonly ILogger<MovieController> _logger;

    public MovieController(IMovieService movieService, ILogger<MovieController> logger) {
        _movieService = movieService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAll() {
        var movies = await _movieService.GetAll();

        if (movies is null) {
            _logger.LogInformation("Movies not found");
            return NotFound();
        }
        
        _logger.LogInformation("Movies fetched");
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Movie>> GetById(Guid id) {
        var movie = await _movieService.GetById(id);
        
        if (movie is null) {
            _logger.LogInformation("Movie with id '{Id}' not found", id.ToString());
            return NotFound($"Movie with id '{id.ToString()}' not found");
        }

        _logger.LogInformation("Movie: {@Movie}", movie);
        return Ok(movie);
    }

    [HttpPost]
    public async Task<ActionResult<MovieCreateResponseModel>> CreateMovie(MovieCreateModel movieCreateModel) {
        try {
            var movie = await _movieService.Create(movieCreateModel);
            return Created($"~api/movie/{movie.Id}", movie);
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (Exception e) {
            _logger.LogError("{E}", e.StackTrace);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMovie(Guid id, MovieUpdateModel movieUpdateModel) {
        try {
            await _movieService.Update(id, movieUpdateModel);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (DbUpdateConcurrencyException e) {
            _logger.LogError("{E}", e.StackTrace);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMovie(Guid id) {
        try {
            await _movieService.Delete(id);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
    }
}