using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieAppAPI.Config;
using MovieAppAPI.Data;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Services.Auth;

public class TokenService : ITokenService {
    private readonly MovieDataContext _context;

    public TokenService(MovieDataContext context, IUserService userService) {
        _context = context;
    }

    public async Task<bool> SaveToken(string token) {
        var tokenModel = new InvalidToken { Token = token };
        _context.ValidTokens.Add(tokenModel);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public async Task<bool> IsTokenValid(string stringToken) {
        var dbToken = await _context.ValidTokens.SingleOrDefaultAsync(token => token.Token == stringToken);
        return dbToken is null;
    }

    private static bool IsTokenExpired(JwtSecurityToken jwt) {
        return DateTime.UtcNow > jwt.ValidTo;
    }

    public async Task<bool> Delete(InvalidToken token) {
        _context.ValidTokens.Remove(token);
        var result = await _context.SaveChangesAsync();

        return result > 0;
    }

    public string GenerateToken(string id) {
        var claimsIdentity = GetIdentity(id);

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

    private static ClaimsIdentity GetIdentity(string id) {
        var claims = new List<Claim> {
            new(JwtRegisteredClaimNames.Sub, id)
        };
        var claimsIdentity = new ClaimsIdentity(claims, "Token");

        return claimsIdentity;
    }
}