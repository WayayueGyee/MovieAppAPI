using System.ComponentModel.DataAnnotations;
using MovieAppAPI.Entities;

namespace MovieAppAPI.Models.User;

public class UserUpdateModel {
    public UserUpdateModel(string? userName, string? email, string? password, DateTime? birthDate, Gender? gender) {
        UserName = userName;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        Gender = gender;
    }

    public string? UserName { get; set; }
    [EmailAddress] public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime? BirthDate { get; set; }
    [EnumDataType(typeof(Gender))] public Gender? Gender { get; set; }
}