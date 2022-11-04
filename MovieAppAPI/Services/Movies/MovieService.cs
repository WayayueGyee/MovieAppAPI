using System.Security.Policy;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Services.Movies;

public class MovieService : IMovieService {
    private readonly MovieDataContext _context;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public MovieService(MovieDataContext context, IUserService userService, IMapper mapper) {
        _context = context;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Movie>?> GetAll() {
        if (_context.Movies is null) {
            return null;
        }
        
        return await _context.Movies.ToListAsync();
    }

    public async Task<Movie?> GetById(Guid id) {
        var movie = await _context.Movies.FindAsync(id);
        return movie;
    }

    public Task<IEnumerable<Movie>> GetAllUserFavourites(Guid userId) {
        throw new NotImplementedException();
    }

    private async Task<bool> IsMovieExists(Guid id) {
        return await _context.Movies.AnyAsync(movie => movie.Id == id);
    }
    
    public async Task<CreateMovieResponseModel> Create(CreateMovieModel createMovieModel) {
        var newMovie = _mapper.Map<Movie>(createMovieModel);
        // TODO: add countryId by country name
        _context.Movies.Add(newMovie);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<CreateMovieResponseModel>(newMovie);
        return response;
    }

    public async Task Update(Guid id, UpdateMovieModel updateMovieModel) {
        var dbMovie = await _context.Movies.FindAsync(id);

        if (dbMovie is null) {
            throw ExceptionHelper.MovieNotFoundException(id.ToString());
        }

        // TODO: ask why it's works like that
        var oldTime = dbMovie.Time;
        var updateMovie = _mapper.Map(updateMovieModel, dbMovie);

        if (updateMovieModel.Time is null) {
            updateMovie.Time = oldTime;
        }
        
        _context.Update(updateMovie);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(Guid id) {
        var isExists = await IsMovieExists(id);
        if (!isExists) {
            throw ExceptionHelper.MovieNotFoundException(id: id.ToString());
        }

        var movie = new Movie(id);
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
    }
}