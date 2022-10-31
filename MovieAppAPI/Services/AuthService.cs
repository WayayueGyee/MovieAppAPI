using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Config;
using MovieAppAPI.Data;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.User;
using MovieAppAPI.Utils;

namespace MovieAppAPI.Services;

public interface IAuthService {
    Task<string> Register(UserRegisterModel registerModel);
    string Login(UserLoginModel loginModel);
}

public class AuthService : IAuthService {
    private readonly IUserService _userService;
    private MovieDataContext _context;
    private readonly IMapper _mapper;

    public AuthService(IUserService userService, MovieDataContext context, IMapper mapper) {
        _userService = userService;
        _context = context;
        _mapper = mapper;
    }

    // TODO: ask when to use exceptions 
    // TODO: make logout 
    // TODO: save tokens in database when logging out
    public async Task<string> Register(UserRegisterModel registerModel) {
        if (registerModel.Password != registerModel.PasswordCheck) {
            throw ExceptionHelper.PasswordsDoNotMatch();
        }

        var createModel = _mapper.Map<UserCreateModel>(registerModel);
        // TODO: is it worth to await here???
        await _userService.Create(createModel);
        var token = GenerateToken(registerModel.Email);

        return token;
    }

    public string Login(UserLoginModel loginModel) {
        var user = _userService.GetByUserName(loginModel.UserName);

        if (Hashing.ComputeSha256Hash(loginModel.Password) != user.PasswordHash) {
            throw ExceptionHelper.PasswordsDoNotMatch();
        }

        var token = GenerateToken(user.Email);

        return token;
    }

    private string GenerateToken(string email) {
        var claimsIdentity = GetIdentity(email);

        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: TokenConfig.Issuer,
            audience: TokenConfig.Audience,
            notBefore: now,
            claims: claimsIdentity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(TokenConfig.Lifetime)),
            signingCredentials: new SigningCredentials(TokenConfig.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }

    private ClaimsIdentity GetIdentity(string email) {
        var user = _userService.GetByEmail(email);
        Console.WriteLine(user.Email);

        var claims = new List<Claim> {
            new(ClaimsIdentity.DefaultNameClaimType, user.Email)
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);

        return claimsIdentity;
    }
}