using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Entities;

[Table("movie")]
public class Movie {
    public Movie() {
        Id = Guid.NewGuid();
        Reviews = new List<Review>();
        Genres = new List<Genre>();
    }

    public Movie(Guid id) {
        Id = id;
        Reviews = new List<Review>();
        Genres = new List<Genre>();
    }

    public Movie(Guid id, string? name, string? poster, int year, TimeSpan time, string? tagline, string? description,
        string? director, int? budget, int? fees, int ageLimit, Guid countryId, Country? country, List<Review> reviews,
        List<Genre> genres) {
        Id = id;
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
        CountryId = countryId;
        Country = country;
        Reviews = reviews;
        Genres = genres;
    }

    [Key] public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Poster { get; set; }
    [Required] public int Year { get; set; }
    [Required] public TimeSpan Time { get; set; }
    public string? Tagline { get; set; }
    public string? Description { get; set; }
    public string? Director { get; set; }
    public int? Budget { get; set; }
    public int? Fees { get; set; }
    public int AgeLimit { get; set; }


    public Guid? CountryId { get; set; }
    public Country? Country { get; set; }

    public List<Review>? Reviews { get; set; }
    public List<Genre>? Genres { get; set; }
}