using MovieAppAPI.Entities;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Services.Reviews; 

public class ReviewService : IReviewService {
    public Task<IEnumerable<Review>?> GetAll() {
        throw new NotImplementedException();
    }

    public Task<Review?> GetById(Guid id) {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Review>> GetAllUserFavourites(Guid userId) {
        throw new NotImplementedException();
    }

    public Task<CreateReviewResponseModel> Create(CreateReviewModel createMovieModel) {
        throw new NotImplementedException();
    }

    public Task Update(Guid id, UpdateReviewModel updateMovieModel) {
        throw new NotImplementedException();
    }

    public Task Delete(Guid id) {
        throw new NotImplementedException();
    }
}