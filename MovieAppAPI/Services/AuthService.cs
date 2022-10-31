using AutoMapper;
using MovieAppAPI.Data;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.User;
using MovieAppAPI.Utils;

namespace MovieAppAPI.Services;

public interface IAuthService {
    Task<string> Register(UserRegisterModel registerModel);
    Task<string> Login(UserLoginModel loginModel);
    Task<bool> Logout(UserLogoutModel logoutModel);
}

public class AuthService : IAuthService {
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly MovieDataContext _context;
    private readonly IMapper _mapper;

    public AuthService(IUserService userService, MovieDataContext context, IMapper mapper, ITokenService tokenService) {
        _userService = userService;
        _context = context;
        _mapper = mapper;
        _tokenService = tokenService;
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
        var token = _tokenService.GenerateToken(registerModel.Email);
        await _tokenService.SaveToken(token);

        return token;
    }

    public async Task<string> Login(UserLoginModel loginModel) {
        var user = _userService.GetByUserName(loginModel.UserName);

        if (Hashing.ComputeSha256Hash(loginModel.Password) != user.PasswordHash) {
            throw ExceptionHelper.PasswordsDoNotMatch();
        }

        var token = _tokenService.GenerateToken(user.Email);
        await _tokenService.SaveToken(token);

        return token;
    }

    public async Task<bool> Logout(UserLogoutModel logoutModel) {
        var token = _mapper.Map<ValidToken>(logoutModel);
        var result = await _tokenService.Delete(token);

        return result;
    }
}