using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Config;
using MovieAppAPI.Data;
using MovieAppAPI.Entities.Auth;

namespace MovieAppAPI.Services;

public interface ITokenService {
    string GenerateToken(string email);
    Task<bool> IsTokenValid(string token);
    Task<bool> SaveToken(string token);
    Task<bool> Delete(ValidToken token);
}

public class TokenService : ITokenService {
    private readonly MovieDataContext _context;
    private readonly IUserService _userService;

    public TokenService(MovieDataContext context, IUserService userService) {
        _context = context;
        _userService = userService;
    }
    
    public async Task<bool> SaveToken(string token) {
        var tokenModel = new ValidToken { Token = token };
        _context.ValidTokens.Add(tokenModel);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
    
    public async Task<bool> IsTokenValid(string token) {
        var dbToken = await _context.ValidTokens.FindAsync(token);

        return dbToken is not null;
    }

    public async Task<bool> Delete(ValidToken token) {
        _context.ValidTokens.Remove(token);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }
    
    public string GenerateToken(string email) {
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