using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models;
using MovieAppAPI.Models.User;
using MovieAppAPI.Utils;

namespace MovieAppAPI.Services;

public interface IUserService {
    IEnumerable<User> GetAll();
    Task<User> GetById(Guid id);
    User GetByEmail(string email);
    User GetByUserName(string userName);
    Task<bool> Create(UserCreateModel user);
    Task<bool> Update(Guid id, UserUpdateModel user);
    Task Delete(Guid id);
    Task Delete(string email);
    Task<bool> IsUserExists(Guid id);
    Task<bool> IsUserExists(string email);
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

    public async Task<bool> IsUserExists(Guid id) {
        return await _context.Users.AnyAsync(user => user.Id == id);
    }

    public async Task<bool> IsUserExists(string email) {
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

    public IEnumerable<User> GetAll() {
        return _context.Users;
    }

    public async Task<User> GetById(Guid id) {
        var user = await _context.Users.FindAsync(id);

        // TODO: find out if it's worth throwing out exception in get operations
        if (user is null) {
            throw ExceptionHelper.UserNotFoundException(id.ToString());
        }

        return user;
    }

    public User GetByEmail(string email) {
        var user = _context.Users.Where(u => u.Email == email).ToList()[0];

        if (user is null) {
            throw ExceptionHelper.UserNotFoundException(email: email);
        }

        return user;
    }

    public User GetByUserName(string userName) {
        var user = _context.Users.Where(u => u.Email == userName).ToList()[0];

        if (user is null) {
            throw ExceptionHelper.UserNotFoundException(userName: userName);
        }

        return user;
    }

    public async Task<bool> Create(UserCreateModel user) {
        var isExists = await IsUserExists(user);
        if (isExists) {
            throw ExceptionHelper.UserAlreadyExistsException();
        }

        var newUser = _mapper.Map<User>(user);

        newUser.PasswordHash = Hashing.ComputeSha256Hash(user.Password);

        _context.Users.Add(newUser);
        var result = await _context.SaveChangesAsync();
        return result > 0;
    }

    public async Task<bool> Update(Guid id, UserUpdateModel user) {
        var dbUser = await _context.Users.FindAsync(id);

        if (dbUser is null) {
            throw ExceptionHelper.UserNotFoundException(id.ToString());
        }

        // TODO: ask why ignoring null in automapping doesn't work with BirthDate = null
        var oldBirthDate = dbUser.BirthDate;
        var newUser = _mapper.Map(user, dbUser);

        if (user.BirthDate is null) {
            newUser.BirthDate = oldBirthDate;
        }

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