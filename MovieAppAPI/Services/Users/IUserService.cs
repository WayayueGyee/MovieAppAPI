using MovieAppAPI.Entities.Users;
using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Services.Users; 

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