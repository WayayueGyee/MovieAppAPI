using System.ComponentModel.DataAnnotations;
using MovieAppAPI.Entities;

namespace MovieAppAPI.Models; 

public class UserUpdateModel {
    public UserUpdateModel(string? userName, string? email, string? password, DateTime? birthDate, string? gender) {
        UserName = userName;
        Email = email;
        Password = password;
        BirthDate = birthDate;
        Gender = gender;    
    }

    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime? BirthDate { get; set; }
    [EnumDataType(typeof(Gender))]
    public string? Gender { get; set; }
}