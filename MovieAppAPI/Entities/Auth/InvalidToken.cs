using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc;

namespace MovieAppAPI.Entities.Auth; 

[Table("valid_token")]
public class ValidToken {
    [Key]
    public string Token { get; set; }
}