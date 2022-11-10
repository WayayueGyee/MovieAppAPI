using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Models.Reviews;

public class ReviewModel {
    public ReviewModel() {
        Author = new UserShortModel();
    }
    
    public ReviewModel(Guid id, int rating, string? reviewText, bool isAnonymous, DateTime createDateTime, UserShortModel author) {
        Id = id;
        Rating = rating;
        ReviewText = reviewText;
        IsAnonymous = isAnonymous;
        CreateDateTime = createDateTime;
        Author = author;
    }
    public Guid Id { get; set; }
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime CreateDateTime { get; set; }
    public UserShortModel? Author { get; set; }
}