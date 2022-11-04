using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Services.Reviews;

public class ReviewService : IReviewService {
    private readonly MovieDataContext _context;
    private readonly IMapper _mapper;

    public ReviewService(MovieDataContext context, IMapper mapper) {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Review>?> GetAll() {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<Review?> GetById(Guid id) {
        var review = await _context.Reviews.FindAsync(id);
        return review;
    }

    private async Task<bool> IsReviewExists(Guid id) {
        return await _context.Reviews.AnyAsync(review => review.Id == id);
    }

    public Task<ReviewCreateResponseModel> Create(ReviewCreateModel reviewCreateMovieModel) {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, ReviewUpdateModel reviewUpdateMovieModel) {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id) {
        throw new NotImplementedException();
    }
}