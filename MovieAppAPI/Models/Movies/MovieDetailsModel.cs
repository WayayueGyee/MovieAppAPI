using MovieAppAPI.Entities;

namespace MovieAppAPI.Models.Movies;

public class MovieDetailsModel {
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
    public string? Country { get; set; }
    public List<Review>? Reviews { get; set; }
    public List<Genre>? Genres { get; set; }
}