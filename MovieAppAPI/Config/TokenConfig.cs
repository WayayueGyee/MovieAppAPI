using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace MovieAppAPI.Config;

public class TokenConfig {
    public const string Issuer = "MovieAppServerHITs";      // Token creator
    public const string Audience = "MovieAppClientHITs";    // Token consumer
    private const string Key = "some12S4__secret)))KeYYY";  // Secret key
    public const int Lifetime = 5;                          // Time in minutes

    public static SymmetricSecurityKey GetSymmetricSecurityKey() {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}