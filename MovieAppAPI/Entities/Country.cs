using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Entities;

public class Country {
    public Country(Guid id, string countryName) {
        Id = id;
        CountryName = countryName;
    }

    [Key] public Guid Id { get; set; }
    public string CountryName { get; set; }
}