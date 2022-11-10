using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Entities.Users;

// Add Avatar attribute
[Table("user")]
[Index(nameof(Email), IsUnique = true)]
[Index(nameof(UserName), IsUnique = true)]
public class User {
    [Key] public Guid Id { get; set; }
    [Required] public string Name { get; set; }
    [Required] public string UserName { get; set; }
    [JsonIgnore] [Required] public string PasswordHash { get; set; }
    [EmailAddress] [Required] public string Email { get; set; }
    public DateTime? BirthDate { get; set; }
    public Gender? Gender { get; set; }
    public string? Avatar { get; set; }
    public Role Role { get; set; } = Role.User;

    public List<FavoriteMovie>? FavoriteMovies { get; set; }

    public User(string userName, string passwordHash, string email, DateTime birthDate, Gender gender, string name,
        Role role) {
        Id = Guid.NewGuid();
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        Name = name;
        Role = role;
    }

    public User(Guid id) {
        Id = id;
        Name = "";
        UserName = "";
        PasswordHash = "";
        Email = "";
    }

    public User() {
        Name = "";
        Id = Guid.Empty;
        UserName = "";
        PasswordHash = "";
        Email = "";
    }

    public static explicit operator User(UserShortModel userShortModel) {
        return new User {
            Id = userShortModel.Id,
            Avatar = userShortModel.Avatar,
            UserName = userShortModel.UserName ?? "simple"
        };
    }
}