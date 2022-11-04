using MovieAppAPI.Entities;
using MovieAppAPI.Models.Movies;

namespace MovieAppAPI.Services.Movies;

public interface IMovieService {
    Task<IEnumerable<Movie>?> GetAll();    
    Task<Movie?> GetById(Guid id);
    Task<IEnumerable<Movie>> GetAllUserFavourites(Guid userId);
    Task<CreateMovieResponseModel> Create(CreateMovieModel createMovieModel); 
    Task Update(Guid id, UpdateMovieModel updateMovieModel);
    Task Delete(Guid id);
}