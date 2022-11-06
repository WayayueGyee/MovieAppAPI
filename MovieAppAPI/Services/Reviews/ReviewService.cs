using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Reviews;
using MovieAppAPI.Services.Movies;
using MovieAppAPI.Services.Users;

namespace MovieAppAPI.Services.Reviews;

public class ReviewService : IReviewService {
    private readonly MovieDataContext _context;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly IMovieService _movieService;

    public ReviewService(MovieDataContext context, IMapper mapper, IMovieService movieService,
        IUserService userService) {
        _context = context;
        _mapper = mapper;
        _movieService = movieService;
        _userService = userService;
    }

    public async Task<IEnumerable<ReviewModel>?> GetAll() {
        var reviewsModelList = await _context.Reviews.Select(review => _mapper.Map<ReviewModel>(review)).ToListAsync();
        return reviewsModelList;
    }

    public async Task<ReviewModel?> GetById(Guid id) {
        var review = await _context.Reviews.FindAsync(id);
        var reviewModel = _mapper.Map<ReviewModel>(review);
        return reviewModel;
    }

    public async Task<ReviewModel> Create(string movieId, string userId, ReviewCreateModel reviewCreateModel) {
        var review = _mapper.Map<Review>(reviewCreateModel);
        review.Movie = await _movieService.GetById(Guid.Parse(movieId)) ??
                       throw ExceptionHelper.MovieNotFoundException(movieId);
        review.Author = await _userService.GetById(Guid.Parse(userId)) ??
                        throw ExceptionHelper.UserNotFoundException(userId);
        review.CreateDateTime = DateTime.UtcNow;

        _context.Reviews.Add(review);
        await _context.SaveChangesAsync();

        var reviewModel = _mapper.Map<ReviewModel>(review);
        return reviewModel;
    }

    private async Task<bool> IsReviewExists(Guid id) {
        return await _context.Reviews.AnyAsync(review => review.Id == id);
    }

    public Task Update(Guid id, ReviewUpdateModel reviewUpdateMovieModel) {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id) {
        throw new NotImplementedException();
    }
}