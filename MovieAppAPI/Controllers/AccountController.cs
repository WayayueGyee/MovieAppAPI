using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.Users;
using MovieAppAPI.Services.Auth;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Controllers;

[Route("api/account/")]
[Produces("application/json")]
[ApiController]
public class AccountController : ControllerExtractToken {
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IAuthService authService, IUserService userService, ILogger<AccountController> logger) {
        _authService = authService;
        _userService = userService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRegisterModel registerModel) {
        try {
            var response = await _authService.Register(registerModel);
            return Created($"~/api/user/{response.Id}", response.Token);
        }
        catch (ObjectsAreNotEqual e) {
            return Unauthorized(e.Message);
        }
        catch (AlreadyExistsException e) {
            return Conflict(e.Message);
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserLoginModel loginModel) {
        try {
            var token = await _authService.Login(loginModel);
            var response = new { token };
            return Ok(response);
        }
        catch (ObjectsAreNotEqual e) {
            return Unauthorized(e.Message);
        }
        catch (AlreadyExistsException e) {
            return Conflict(e.Message);
        }
        catch (RecordNotFoundException e) {
            return NotFound(e.Message);
        }
    }

    [HttpPost("logout")]
    [Authorize("TokenNotRejected")]
    public async Task<IActionResult> Logout() {
        var token = AuthService.TokenFromHeader(Request.Headers);
        if (token is null) {
            return Unauthorized();
        }

        try {
            await _authService.Logout(token);
            return Ok();
        }
        catch (Exception e) {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("profile")]
    [Authorize("TokenNotRejected")]
    public async Task<ActionResult<ProfileModel>> GetProfile() {
        try {
            var id = GetUserIdFromToken();
            var profile = await _userService.GetProfile(id);
            return Ok(profile);
        }
        catch (InvalidTokenException e) {
            _logger.LogInformation("{E}", e.Message);
            return Unauthorized();
        }
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(ProfileUpdateModel profileModel) {
        try {
            var id = GetUserIdFromToken();
            await _userService.UpdateProfile(id, profileModel);
            return NoContent();
        }
        catch (InvalidTokenException e) {
            _logger.LogInformation("{E}", e.Message);
            return Unauthorized();
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
}