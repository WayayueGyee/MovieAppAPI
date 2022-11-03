using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MovieAppAPI.Config;

public static class TokenConfig {
    public const string Issuer = "MovieAppServerHITs";      // Token creator
    public const string Audience = "MovieAppClientHITs";    // Token consumer
    private const string Key = "some12S4__secret)))KeYYY";  // Secret key
    public const int Lifetime = 10;                          // Time in minutes

    public static SymmetricSecurityKey GetSymmetricSecurityKey() {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }

    public static TokenValidationParameters CreateValidationParameters() {
        return new TokenValidationParameters {
            ClockSkew = TimeSpan.Zero,
            
            ValidateIssuer = true,
            ValidIssuer = Issuer,

            ValidateAudience = true,
            ValidAudience = Audience,

            ValidateLifetime = true,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = GetSymmetricSecurityKey(),
        };
    }
}