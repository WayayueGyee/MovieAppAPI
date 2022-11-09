using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
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

    private async Task<bool> IsReviewExists(Guid id) {
        return await _context.Reviews.AnyAsync(review => review.Id == id);
    }

    public async Task<IEnumerable<ReviewModel>?> GetAll() {
        var reviewsModelList = await _context.Reviews.Include(model => model.Author)
            .Select(review => _mapper.Map<ReviewModel>(review)).ToListAsync();
        return reviewsModelList;
    }

    public async Task<ReviewModel?> GetById(Guid id) {
        // WARNING: Не факт, что это работает, так как здесь нет Include
        var review = await _context.Reviews.FindAsync(id);
        var reviewModel = _mapper.Map<ReviewModel>(review);
        return reviewModel;
    }

    /// <exception cref="RecordNotFoundException"></exception>
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

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Update(Guid id, ReviewUpdateModel reviewUpdateModel) {
        var dbReview = await _context.Reviews.FindAsync(id);

        if (dbReview is null) {
            throw ExceptionHelper.ReviewNotFoundException(id.ToString());
        }

        var updatedReview = _mapper.Map(reviewUpdateModel, dbReview);
        _context.Update(updatedReview);
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Update(Guid movieId, Guid reviewId, Guid userId, ReviewUpdateModel reviewUpdateModel) {
        var dbReview = await _context.Reviews.FindAsync(reviewId);

        if (dbReview is null) {
            throw ExceptionHelper.ReviewNotFoundException(id: reviewId.ToString());
        }

        if (dbReview.Author.Id != userId) {
            throw ExceptionHelper.PermissionsDeniedException();
        }

        var updatedReview = _mapper.Map(reviewUpdateModel, dbReview);
        _context.Update(updatedReview);
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Delete(Guid id) {
        var isExists = await IsReviewExists(id);
        if (!isExists) {
            throw ExceptionHelper.ReviewNotFoundException(id: id.ToString());
        }

        var review = new Review(id);
        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }

    /// <exception cref="RecordNotFoundException"></exception>
    public async Task Delete(Guid movieId, Guid reviewId) {
        var review = await _context.Reviews.FindAsync(reviewId);

        if (review is null) {
            throw ExceptionHelper.ReviewNotFoundException(id: reviewId.ToString());
        }

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}