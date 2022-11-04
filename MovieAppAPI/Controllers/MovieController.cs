using System.Net;
using Microsoft.AspNetCore.Mvc;
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
        
        _logger.LogInformation("Get all movies");
        return Ok(movies);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) {
        var movie = await _movieService.GetById(id);
        if (movie is null) {
            _logger.LogInformation("Movie with id \"{Id}\" not found", id.ToString());
            return NotFound($"Movie with id \"{id.ToString()}\" not found");
        }

        _logger.LogInformation("Movie: {@Movie}", movie);
        return Ok(movie);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMovie(CreateMovieModel createMovieModel) {
        try {
            var movie = await _movieService.Create(createMovieModel);
            return Created($"~api/movie/{movie.Id}", movie);
        }
        catch (InvalidOperationException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        catch (Exception e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMovie(Guid id, UpdateMovieModel updateMovieModel) {
        try {
            await _movieService.Update(id, updateMovieModel);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMovie(Guid id) {
        try {
            await _movieService.Delete(id);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
    }
}