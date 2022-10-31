using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieAppAPI.Models.Auth;

public class UserLoginModel {
    [Required]
    [JsonPropertyName("username")]
    public string UserName { get; set; }

    [Required]
    public string Password { get; set; }
}