using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Entities;

[Table("movie_genre")]
public class MovieGenre {
    [Key] public Guid MovieId { get; set; }
    public Movie Movie { get; set; }

    [Key] public Guid GenreId { get; set; }
    public Genre Genre { get; set; }
}