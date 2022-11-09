using System.ComponentModel.DataAnnotations;
using MovieAppAPI.Entities.Users;

namespace MovieAppAPI.Models.Users;

public class UserUpdateModel {
    public string? UserName { get; set; }
    [EmailAddress] public string? Email { get; set; }
    public string? Password { get; set; }
    public DateTime? BirthDate { get; set; }
    [EnumDataType(typeof(Gender))] public Gender? Gender { get; set; }
}