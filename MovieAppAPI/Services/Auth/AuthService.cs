using AutoMapper;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Auth;
using MovieAppAPI.Models.Users;
using MovieAppAPI.Services.Users;
using MovieAppAPI.Utils;

namespace MovieAppAPI.Services.Auth;

public class AuthService : IAuthService {
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public AuthService(IUserService userService, IMapper mapper, ITokenService tokenService) {
        _userService = userService;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    // TODO: ask when to use exceptions 
    // TODO: make logout 
    // TODO: save tokens in database when logging out
    /// <exception cref="ObjectsAreNotEqual"></exception>
    public async Task<string> Register(UserRegisterModel registerModel) {
        if (registerModel.Password != registerModel.PasswordCheck) {
            throw ExceptionHelper.PasswordsDoNotMatch();
        }

        var createModel = _mapper.Map<UserCreateModel>(registerModel);
        // TODO: is it worth to await here???
        await _userService.Create(createModel);
        var token = _tokenService.GenerateToken(registerModel.Email);

        return token;
    }

    /// <exception cref="ObjectsAreNotEqual"></exception>
    public Task<string> Login(UserLoginModel loginModel) {
        var user = _userService.GetByUserName(loginModel.UserName);

        if (Hashing.ComputeSha256Hash(loginModel.Password) != user.PasswordHash) {
            throw ExceptionHelper.PasswordsDoNotMatch();
        }

        var token = _tokenService.GenerateToken(user.Email);

        return Task.FromResult(token);
    }

    public async Task Logout(string stringToken) {
        var token = new InvalidToken { Token = stringToken };
        await _tokenService.SaveToken(token.Token);
    }

    public static string? TokenFromHeader(IHeaderDictionary headers) {
        if (!headers.ContainsKey("Authorization")) return null;

        string authHeader = headers.Authorization;

        if (string.IsNullOrEmpty(authHeader)) return null;

        const string authType = "bearer";

        if (!authHeader.StartsWith(authType, StringComparison.OrdinalIgnoreCase)) return null;

        var token = authHeader[authType.Length..];

        return string.IsNullOrEmpty(token) ? null : token;
    }
}