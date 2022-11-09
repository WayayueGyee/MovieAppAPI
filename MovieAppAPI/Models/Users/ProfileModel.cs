using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MovieAppAPI.Entities.Users;

namespace MovieAppAPI.Models.Users;

public class ProfileModel {
    public Guid Id { get; set; }
    [JsonPropertyName("nickName")] public string? UserName { get; set; }
    public string Email { get; set; }
    [JsonPropertyName("avatarLink")] public string? Avatar { get; set; }
    public string Name { get; set; }
    public DateTime BirthDate { get; set; }
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }
}