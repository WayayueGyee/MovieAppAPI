namespace MovieAppAPI.Models.Reviews; 

public class ReviewCreateModel {
    public int Rating { get; set; }
    public string? ReviewText { get; set; }
    public bool IsAnonymous { get; set; }
}