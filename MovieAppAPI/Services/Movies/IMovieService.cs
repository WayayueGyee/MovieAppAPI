using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Movies;

namespace MovieAppAPI.Services.Movies;

public interface IMovieService {
    Task<IEnumerable<Movie>?> GetAll();
    Task<Movie?> GetById(Guid id);
    Task<IEnumerable<Movie>> GetAllUserFavourites(Guid userId);
    Task<MovieCreateResponseModel> Create(MovieCreateModel movieCreateModel);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Update(Guid id, MovieUpdateModel movieUpdateModel);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Delete(Guid id);
}