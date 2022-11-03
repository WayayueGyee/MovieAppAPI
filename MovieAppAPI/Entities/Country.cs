using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Entities;

[Table("country")]
public class Country {
    [Key] public Guid Id { get; set; }
    public string CountryName { get; set; }
}