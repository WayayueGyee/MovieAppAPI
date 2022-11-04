using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MovieAppAPI.Entities;

[Table("country")]
[Index("CountryName", IsUnique = true)]
public class Country {
    public Country() {
        Id = Guid.Empty;
        CountryName = "";
    }

    public Country(Guid id) {
        Id = id;
        CountryName = "";
    }

    [Key] public Guid Id { get; set; }
    public string CountryName { get; set; }
}