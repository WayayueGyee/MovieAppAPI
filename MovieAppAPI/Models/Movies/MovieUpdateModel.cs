namespace MovieAppAPI.Models.Movies;

public class MovieUpdateModel {
    public string? Name { get; set; }
    public string? Poster { get; set; }
    public int? Year { get; set; }
    public TimeSpan? Time { get; set; }
    public string? Tagline { get; set; }
    public string? Description { get; set; }
    public string? Director { get; set; }
    public int? Budget { get; set; }
    public int? Fees { get; set; }
    public int? AgeLimit { get; set; }
    public string? CountryName { get; set; }
}