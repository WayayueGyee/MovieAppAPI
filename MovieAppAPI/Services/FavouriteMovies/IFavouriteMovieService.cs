using System.Diagnostics;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Movies;

namespace MovieAppAPI.Services.FavouriteMovies;

public interface IFavoriteMovieService {
    Task<List<MovieElementModel>?> GetAll(Guid userId);

    /// <exception cref="AlreadyExistsException"></exception>
    Task Create(Guid movieId, Guid userId);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Delete(Guid movieId, Guid userId);
}