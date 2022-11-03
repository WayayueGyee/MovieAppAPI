using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Models.Users;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Controllers;

[Route("api/user")]
[Produces("application/json")]
[ApiController]
[Authorize("TokenNotRejected")]
public class UserController : ControllerBase {
    private readonly IUserService _userService;

    public UserController(IUserService userService) {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult GetAll() {
        return Ok(_userService.GetAll());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) {
        return Ok(await _userService.GetById(id));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(UserCreateModel user) {
        var result = await _userService.Create(user);
        var message = result ? "User was successfully created" : "Something went wrong";
        return Ok(message);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserUpdateModel user) {
        var result = await _userService.Update(id, user);
        var message = result ? "User was successfully updated" : "Something went wrong";
        return Ok(message);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser(Guid id) {
        await _userService.Delete(id);
        return Ok("User was deleted");
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteUser([FromQuery(Name = "email")] string email) {
        await _userService.Delete(email);
        return Ok("User was deleted");
    }
}