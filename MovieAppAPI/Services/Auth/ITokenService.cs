using MovieAppAPI.Entities.Auth;

namespace MovieAppAPI.Services.Auth; 

public interface ITokenService {
    string GenerateToken(string id);
    Task<bool> IsTokenValid(string token);
    Task<bool> SaveToken(string token);
    Task<bool> Delete(InvalidToken token);
}