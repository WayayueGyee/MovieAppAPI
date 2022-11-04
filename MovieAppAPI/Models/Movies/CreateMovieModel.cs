using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models.Movies;

public class CreateMovieModel {
    public CreateMovieModel(string? name, string? poster, int year, TimeSpan time, string? tagline, string? description,
        string? director, int? budget, int? fees, int ageLimit, string? countryName) {
        Name = name;
        Poster = poster;
        Year = year;
        Time = time;
        Tagline = tagline;
        Description = description;
        Director = director;
        Budget = budget;
        Fees = fees;
        AgeLimit = ageLimit;
        CountryName = countryName;
    }

    public string? Name { get; set; }
    public string? Poster { get; set; }
    [Required] public int Year { get; set; }
    [Required] public TimeSpan Time { get; set; }
    public string? Tagline { get; set; }
    public string? Description { get; set; }
    public string? Director { get; set; }
    public int? Budget { get; set; }
    public int? Fees { get; set; }
    [Required] public int AgeLimit { get; set; }
    public string? CountryName { get; set; }
}