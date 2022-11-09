using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieAppAPI.Entities.Users;

namespace MovieAppAPI.Entities;

[Table("favorite_movies")]
public class FavoriteMovie {
    public FavoriteMovie(Movie movie, User user) {
        Movie = movie;
        User = user;
    }

    public FavoriteMovie(Guid movieId, Guid userId) {
        MovieId = movieId;
        UserId = userId;
    }

    [Key] public Guid MovieId { get; set; }
    public Movie Movie { get; set; }

    [Key] public Guid UserId { get; set; }
    public User User { get; set; }
}