using MovieAppAPI.Entities;
using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Services.Reviews;

public interface IReviewService {
    Task<IEnumerable<ReviewModel>?> GetAll();
    Task<ReviewModel?> GetById(Guid id);

    /// <exception cref="RecordNotFoundException"></exception>
    Task<ReviewModel> Create(string movieId, string userId, ReviewCreateModel reviewCreateModel);

    Task Update(Guid id, ReviewUpdateModel reviewUpdateModel);
    Task Delete(Guid id);
}