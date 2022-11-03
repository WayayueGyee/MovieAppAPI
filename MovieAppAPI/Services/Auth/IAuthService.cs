using MovieAppAPI.Models.Auth;

namespace MovieAppAPI.Services.Auth; 

public interface IAuthService {
    Task<string> Register(UserRegisterModel registerModel);
    Task<string> Login(UserLoginModel loginModel);
    Task Logout(string stringToken);
}