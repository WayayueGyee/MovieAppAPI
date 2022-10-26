using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Entities;

[Table("user")]
public class User {
    [Key] public Guid Id { get; set; }
    [Required] public string UserName { get; set; }
    [Required] public string PasswordHash { get; set; }
    [Required] public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public Gender Gender { get; set; }

    public User(string userName, string passwordHash, string email, DateTime birthDate, Gender gender) {
        Id = Guid.NewGuid();
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
    }
}