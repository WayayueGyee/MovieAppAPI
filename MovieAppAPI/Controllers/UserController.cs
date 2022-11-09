using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Users;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Controllers;

[Route("api/user")]
[Produces("application/json")]
[ApiController]
[Authorize("TokenNotRejected")]
public class UserController : ControllerBase {
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger) {
        _userService = userService;
        _logger = logger;
    }
    
    

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(_userService.GetAll());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) {
        var user = await _userService.GetById(id);
        if (user is null) {
            return NotFound($"User with id \"{id.ToString()}\" not found");
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateModel user) {
        try {
            await _userService.Create(user);
            _logger.LogError("Create method executed. User: \n{@User} \ncreated", user);
            return Ok("User was successfully created");
        }
        catch (AlreadyExistsException e) {
            _logger.LogError("{E}", e.StackTrace);
            return NotFound(e.Message);
        }
        catch (DbUpdateException e) {
            _logger.LogError("{E}", e.StackTrace);
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserUpdateModel user) {
        try {
            await _userService.Update(id, user);
            return Ok("User was successfully updated");
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
        catch (DbUpdateException) {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id) {
        try {
            await _userService.Delete(id);
            return Ok("User was deleted");
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
        catch (DbUpdateException) {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery(Name = "email")] string email) {
        try {
            await _userService.Delete(email);
            return Ok("User was deleted");
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
        catch (DbUpdateException) {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}