using MovieAppAPI.Entities;

namespace MovieAppAPI.Models.Movies;

public class CreateMovieResponseModel {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Poster { get; set; }
    public int Year { get; set; }
    public TimeSpan Time { get; set; }
    public string? Tagline { get; set; }
    public string? Description { get; set; }
    public string? Director { get; set; }
    public int? Budget { get; set; }
    public int? Fees { get; set; }
    public int AgeLimit { get; set; }
    public Guid? CountryId { get; set; }
    public List<Genre>? Genres { get; set; }
}