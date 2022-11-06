using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Services.Countries;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Services.Movies;

public class MovieService : IMovieService {
    private readonly MovieDataContext _context;
    private readonly IUserService _userService;
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public MovieService(MovieDataContext context, IUserService userService, IMapper mapper,
        ICountryService countryService) {
        _context = context;
        _userService = userService;
        _mapper = mapper;
        _countryService = countryService;
    }

    public async Task<IEnumerable<Movie>?> GetAll() {
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
    
    private async Task AddCountryId(string? countryName, Movie movie) {
        if (countryName is null) return;

        var country = await _countryService.GetByCountryName(countryName);

        if (country is null) {
            throw ExceptionHelper.CountryNotFoundException(name: countryName);
        }

        movie.CountryId = country.Id;
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task<MovieCreateResponseModel> Create(MovieCreateModel movieCreateModel) {
        var newMovie = _mapper.Map<Movie>(movieCreateModel);

        await AddCountryId(movieCreateModel.CountryName, newMovie);

        _context.Movies.Add(newMovie);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<MovieCreateResponseModel>(newMovie);
        return response;
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Update(Guid id, MovieUpdateModel movieUpdateModel) {
        var dbMovie = await _context.Movies.FindAsync(id);

        if (dbMovie is null) {
            throw ExceptionHelper.MovieNotFoundException(id.ToString());
        }

        // TODO: ask why it's works like that
        var oldTime = dbMovie.Time;
        var updatedMovie = _mapper.Map(movieUpdateModel, dbMovie);

        if (movieUpdateModel.Time is null) {
            updatedMovie.Time = oldTime;
        }

        await AddCountryId(movieUpdateModel.CountryName, updatedMovie);

        _context.Update(updatedMovie);
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
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