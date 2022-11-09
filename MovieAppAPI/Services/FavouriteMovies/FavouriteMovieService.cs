using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Movies;

namespace MovieAppAPI.Services.FavouriteMovies;

public class FavoriteMovieService : IFavoriteMovieService {
    private readonly MovieDataContext _context;
    private readonly IMapper _mapper;

    public FavoriteMovieService(MovieDataContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<MovieElementModel>?> GetAll(Guid userId) {
        var movies = await _context.Movies.Where(movie => movie.FavoriteMovies.Any(fm => fm.UserId == userId))
            .Include(movie => movie.Reviews).Include(movie => movie.Genres)
            .Select(movie => _mapper.Map<MovieElementModel>(movie)).ToListAsync();
        return movies;
    }

    private async Task<bool> IsFavoriteMovieExists(Guid movieId, Guid userId) {
        return await _context.Movies.AnyAsync(movie =>
            movie.FavoriteMovies.Any(fm => fm.MovieId == movieId && fm.UserId == userId));
    }
    
    /// <exception cref="AlreadyExistsException"></exception>
    public async Task Create(Guid movieId, Guid userId) {
        var isExists = await IsFavoriteMovieExists(movieId, userId);
        if (isExists) {
            throw ExceptionHelper.FavoriteMovieAlreadyExistsException(movieId.ToString(), userId.ToString());
        }

        _context.FavoriteMovies.Add(new FavoriteMovie(movieId, userId));
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Delete(Guid movieId, Guid userId) {
        var isExists = await IsFavoriteMovieExists(movieId, userId);
        if (!isExists) {
            throw ExceptionHelper.FavoriteMovieNotFoundException(movieId.ToString(), userId.ToString());
        }

        _context.FavoriteMovies.Remove(new FavoriteMovie(movieId, userId));
        await _context.SaveChangesAsync();
    }
}