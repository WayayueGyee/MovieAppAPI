using MovieAppAPI.Entities.Users;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Services.Users;

public interface IUserService {
    IEnumerable<User> GetAll();
    Task<User?> GetById(Guid id);
    Task<User?> GetByEmail(string email);
    Task<User?> GetByUserName(string userName);
    Task<ProfileModel> GetProfile(Guid id);

    /// <exception cref="RecordNotFoundException"></exception>
    Task UpdateProfile(Guid id, ProfileUpdateModel profileModel);

    Task<User> Create(UserCreateModel user);
    Task Update(Guid id, UserUpdateModel user);
    Task Delete(Guid id);
    Task Delete(string email);
    Task<bool> IsUserExists(Guid id);
    Task<bool> IsUserExists(string email);
}