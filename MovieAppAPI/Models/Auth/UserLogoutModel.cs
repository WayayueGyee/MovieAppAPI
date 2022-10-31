using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models.Auth;

public class UserLogoutModel {
    [Required] public string Token { get; set; }
}