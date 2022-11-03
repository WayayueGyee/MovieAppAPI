using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Entities;

[Table("movie")]
public class Movie {
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


    public Guid CountryId { get; set; }
    public Country? Country { get; set; }

    public List<Review> Reviews { get; set; }
    public List<Genre> Genres { get; set; }
}