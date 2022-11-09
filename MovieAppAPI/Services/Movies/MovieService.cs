using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Models.Pagination;
using MovieAppAPI.Services.Countries;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Services.Movies;

public class MovieService : IMovieService {
    private const int PageSize = 2;
    private readonly MovieDataContext _context;
    private readonly ICountryService _countryService;
    private readonly IMapper _mapper;

    public MovieService(MovieDataContext context, IUserService userService, IMapper mapper,
        ICountryService countryService) {
        _context = context;
        _mapper = mapper;
        _countryService = countryService;
    }

    private int FilmsCount => _context.Movies.Count();
    private int TotalPages => (int)Math.Ceiling(FilmsCount / (double)PageSize);
    private int LastPage { get; set; }
    private string LastId { get; set; }

    public async Task<IEnumerable<Movie>?> GetAll() {
        return await _context.Movies.ToListAsync();
    }

    public async Task<MovieDetailsModel?> GetMovieDetails(Guid id) {
        // TODO: SingleOrDefaultAsync or Where???
        var movie = await _context.Movies.Include(movie => movie.Genres).Include(movie => movie.Reviews)
            .ThenInclude(review => review.Author)
            .SingleOrDefaultAsync(movie => movie.Id == id);

        if (movie is null) return null;

        var movieDetailsModel = _mapper.Map<MovieDetailsModel>(movie);
        return movieDetailsModel;
    }

    public async Task<Movie?> GetById(Guid id) {
        var movie = await _context.Movies.FindAsync(id);
        return movie;
    }

    public async Task<MoviePagedListModel?> GetPage(int currentPage) {
        if (currentPage > TotalPages) {
            return null;
        }

        var moviesToSkip = PageSize * (currentPage - 1);
        
        var nextPage = await _context.Movies
            .OrderBy(movie => movie.Id)
            .Skip(moviesToSkip)
            .Take(PageSize)
            .Include(movie => movie.Reviews)
            .Include(movie => movie.Genres)
            .Select(movie => _mapper.Map<MovieElementModel>(movie))
            .ToListAsync();
        var pageInfo = new PageInfoModel { PageSize = PageSize, CurrentPage = currentPage, TotalPages = TotalPages };

        return new MoviePagedListModel(pageInfo, nextPage);
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task<MovieCreateResponseModel> Create(MovieCreateModel movieCreateModel) {
        var newMovie = _mapper.Map<Movie>(movieCreateModel);

        await AddCountry(movieCreateModel.CountryName, newMovie);

        _context.Movies.Add(newMovie);
        await _context.SaveChangesAsync();

        var response = _mapper.Map<MovieCreateResponseModel>(newMovie);
        return response;
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Update(Guid id, MovieUpdateModel movieUpdateModel) {
        var dbMovie = await _context.Movies.FindAsync(id);

        if (dbMovie is null) throw ExceptionHelper.MovieNotFoundException(id.ToString());

        // TODO: ask why it's works like that
        var oldTime = dbMovie.Time;
        var updatedMovie = _mapper.Map(movieUpdateModel, dbMovie);

        if (movieUpdateModel.Time is null) updatedMovie.Time = oldTime;

        await AddCountry(movieUpdateModel.CountryName, updatedMovie);

        _context.Update(updatedMovie);
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Delete(Guid id) {
        var isExists = await IsMovieExists(id);
        if (!isExists) throw ExceptionHelper.MovieNotFoundException(id.ToString());

        var movie = new Movie(id);
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> IsMovieExists(Guid id) {
        return await _context.Movies.AnyAsync(movie => movie.Id == id);
    }

    private async Task AddCountry(string? countryName, Movie movie) {
        if (countryName is null) return;

        var country = await _countryService.GetByCountryName(countryName);

        if (country is null) throw ExceptionHelper.CountryNotFoundException(name: countryName);

        movie.Country = country;
    }
}