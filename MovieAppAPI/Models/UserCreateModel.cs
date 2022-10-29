using System.ComponentModel.DataAnnotations;
using MovieAppAPI.Entities;

namespace MovieAppAPI.Models;

public class UserCreateModel {
    public UserCreateModel(string userName, string name, string password, string email, DateTime birthDate,
        Gender gender) {
        UserName = userName;
        Name = name;
        Password = password;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
    }

    public string UserName { get; set; }
    public string Name { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }
}