using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

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
    [Required] [EmailAddress] public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }

    public User(string userName, string passwordHash, string email, DateTime birthDate, Gender gender, string name) {
        Id = Guid.NewGuid();
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
        Name = name;
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
}