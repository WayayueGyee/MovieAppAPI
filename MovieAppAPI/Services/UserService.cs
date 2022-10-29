using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models;

namespace MovieAppAPI.Services;

public interface IUserService {
    IEnumerable<User> GetAll();
    Task<User> GetById(Guid id);
    Task<bool> Create(UserCreateModel user);
    Task<bool> Update(Guid id, UserUpdateModel user);
    Task Delete(Guid id);

    Task Delete(string email);
}

public class UserService : IUserService {
    private readonly MovieDataContext _context;
    private readonly IMapper _mapper;

    public UserService(MovieDataContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    private async Task<bool> IsUserExists(UserCreateModel newUser) {
        return await _context.Users.AnyAsync(user => user.Email == newUser.Email || user.UserName == newUser.UserName);
    }

    private async Task<bool> IsUserExists(UserUpdateModel newUser) {
        return await _context.Users.AnyAsync(user => user.Email == newUser.Email || user.UserName == newUser.UserName);
    }

    private async Task<bool> IsUserExists(Guid id) {
        return await _context.Users.AnyAsync(user => user.Id == id);
    }

    private async Task<bool> IsUserExists(string email) {
        return await _context.Users.AnyAsync(user => user.Email == email);
    }

    // private async Task<bool> IsUserExists<TEntity>(TEntity entity) {
    //     var entityType = typeof(TEntity);
    //     var entityProperties = entityType.GetProperties();
    //
    //     foreach (var propertyInfo in entityProperties) {
    //         var propName = propertyInfo.Name.ToLower();
    //
    //         if (propName is "email" or "username") {
    //             
    //         }
    //     }
    // }

    private static string ComputeSha256Hash(string rawString) {
        var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawString));
        var builder = new StringBuilder(bytes.Length);

        foreach (var b in bytes) builder.Append(b.ToString("X"));

        return builder.ToString();
    }

    public IEnumerable<User> GetAll() {
        return _context.Users;
    }

    public async Task<User> GetById(Guid id) {
        var user = await _context.Users.FindAsync(id);

        if (user is null) {
            throw ExceptionHelper.UserNotFoundException(id.ToString());
        }

        return user;
    }

    public async Task<bool> Create(UserCreateModel user) {
        // var isExists = await IsUserExists(user);
        // if (isExists) {
        //     throw ExceptionHelper.UserAlreadyExistsException();
        // }

        var newUser = _mapper.Map<User>(user);

        newUser.PasswordHash = ComputeSha256Hash(user.Password);

        _context.Users.Add(newUser);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> Update(Guid id, UserUpdateModel user) {
        var isExists = await IsUserExists(user);
        if (isExists) {
            throw ExceptionHelper.UserAlreadyExistsException();
        }

        var dbUser = await _context.Users.FindAsync(id);

        if (dbUser is null) {
            throw ExceptionHelper.UserNotFoundException(id.ToString());
        }

        var newUser = _mapper.Map<User>(user);
        _context.Users.Update(newUser);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task Delete(Guid id) {
        var isExists = await IsUserExists(id);
        if (!isExists) {
            throw ExceptionHelper.UserNotFoundException(id.ToString());
        }

        // _context.Users.FromSqlInterpolated($"DELETE FROM \"user\" WHERE \"Id\"='{id.ToString()}'");
        var user = new User(id);
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(string email) {
        var isExists = await IsUserExists(email);
        if (!isExists) {
            throw ExceptionHelper.UserNotFoundException(email);
        }

        // _context.Users.FromSqlInterpolated($"DELETE FROM \"user\" WHERE \"Email\"='{email}'");
        var user = new User { Email = email };
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}