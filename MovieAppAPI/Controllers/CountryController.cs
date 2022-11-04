using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Countries;
using MovieAppAPI.Services.Countries;

namespace MovieAppAPI.Controllers; 

[Route("api/country")]
[ApiController]
public class CountryController : ControllerBase {
    private readonly ICountryService _countryService;
    private readonly ILogger<CountryController> _logger;

    public CountryController(ICountryService countryService, ILogger<CountryController> logger) {
        _countryService = countryService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Country>>> GetAll() {
        var countries = await _countryService.GetAll();

        if (countries is null) {
            _logger.LogInformation("Countries not found");
            return NotFound();
        }

        _logger.LogInformation("Get all countries");
        return Ok(countries);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Country>> GetById(Guid id) {
        var country = await _countryService.GetById(id);
        if (country is null) {
            _logger.LogInformation("Country with id \"{Id}\" not found", id.ToString());
            return NotFound($"Country with id \"{id.ToString()}\" not found");
        }

        _logger.LogInformation("Country: {@Country}", country);
        return Ok(country);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCountry(CountryCreateModel countryCreateModel) {
        try {
            var country = await _countryService.Create(countryCreateModel);
            return Created($"~api/country/{country.Id}", country);
        }
        catch (AlreadyExistsException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return Conflict($"Country with name \"{countryCreateModel.CountryName}\" already exists");
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateMovie(Guid id, CountryUpdateModel countryUpdateModel) {
        try {
            await _countryService.Update(id, countryUpdateModel);
            return NoContent();
        }
        catch (AlreadyExistsException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return Conflict($"Country with name \"{countryUpdateModel.CountryName}\" already exists");
        }
        catch (RecordNotFoundException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (DbUpdateConcurrencyException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteMovie(Guid id) {
        try {
            await _countryService.Delete(id);
            return NoContent();
        }
        catch (RecordNotFoundException e) {
            _logger.LogInformation("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
    }
}