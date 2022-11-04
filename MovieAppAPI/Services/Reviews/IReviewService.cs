using MovieAppAPI.Entities;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Services.Reviews; 

public interface IReviewService {
    Task<IEnumerable<Review>?> GetAll();    
    Task<Review?> GetById(Guid id);
    Task<ReviewCreateResponseModel> Create(ReviewCreateModel reviewCreateModel); 
    Task Update(Guid id, ReviewUpdateModel reviewUpdateModel);
    Task Delete(Guid id);
}