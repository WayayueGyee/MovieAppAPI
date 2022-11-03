using System.ComponentModel.DataAnnotations;
using System.Globalization;
using MovieAppAPI.Entities.Users;

namespace MovieAppAPI.Models.Users;

public class UserCreateModel {
    private const byte MinPasswordLength = 6;

    public UserCreateModel(string userName, string name, string password, string email, DateTime birthDate,
        Gender gender) {
        UserName = userName;
        Name = name;
        Password = password;
        Email = email;
        BirthDate = birthDate;
        Gender = gender;
    }

    [Required] public string UserName { get; set; }
    [Required] [EmailAddress] public string Email { get; set; }
    [Required] public string Name { get; set; }

    [Required]
    [MinLength(MinPasswordLength, ErrorMessage = "Password must be at least 6 symbols long")]
    public string Password { get; set; }

    public DateTime BirthDate { get; set; }

    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }

    public override string ToString() {
        return $"UserName: {UserName},\n" + $"Email: {Email},\n" + $"Name: {Name},\n" +
               $"BirthDate: {BirthDate.ToString(CultureInfo.CurrentCulture)},\n" + $"Gender: {Gender.ToString()}";
    }
}