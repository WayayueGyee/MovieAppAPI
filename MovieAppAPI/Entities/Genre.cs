using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieAppAPI.Models.Genres;

namespace MovieAppAPI.Entities;

[Table("genre")]
public class Genre {
    [Key] public Guid Id { get; set; }
    public string Name { get; set; }

    public List<Movie>? Movies { get; set; }
    public List<MovieGenre> MovieGenres { get; set; }

    public static explicit operator Genre(GenreModel genreModel) {
        return new Genre {
            Id = genreModel.Id,
            Name = genreModel.Name
        };
    }
}