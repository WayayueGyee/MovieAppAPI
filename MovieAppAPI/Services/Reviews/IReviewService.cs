using MovieAppAPI.Exceptions;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Services.Reviews;

public interface IReviewService {
    Task<IEnumerable<ReviewModel>?> GetAll();
    Task<ReviewModel?> GetById(Guid id);

    /// <exception cref="RecordNotFoundException"></exception>
    Task<ReviewModel> Create(string movieId, string userId, ReviewCreateModel reviewCreateModel);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Update(Guid id, ReviewUpdateModel reviewUpdateModel);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Update(Guid movieId, Guid reviewId, Guid userId, ReviewUpdateModel reviewUpdateModel);

    /// <exception cref="RecordNotFoundException"></exception>
    Task Delete(Guid id);

    /// <exception cref="RecordNotFoundException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    Task Delete(Guid movieId, Guid reviewId);
}