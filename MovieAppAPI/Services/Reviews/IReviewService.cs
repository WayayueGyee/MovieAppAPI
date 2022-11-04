using MovieAppAPI.Entities;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Services.Reviews; 

public interface IReviewService {
    Task<IEnumerable<Review>?> GetAll();    
    Task<Review?> GetById(Guid id);
    Task<IEnumerable<Review>> GetAllUserFavourites(Guid userId);
    Task<CreateReviewResponseModel> Create(CreateReviewModel createMovieModel); 
    Task Update(Guid id, UpdateReviewModel updateMovieModel);
    Task Delete(Guid id);
}