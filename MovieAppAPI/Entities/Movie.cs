using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Entities; 

public class Movie {
    [Key] public Guid Id { get; set; }
    public string Name { get; set; }
    public string Poster { get; set; }
    public int Year { get; set; }
    public Country Country { get; set; }
    public Genre Genre { get; set; }
}