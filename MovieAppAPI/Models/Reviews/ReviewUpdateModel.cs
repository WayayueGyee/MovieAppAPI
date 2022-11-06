using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models.Reviews;

public class ReviewUpdateModel {
    public string? ReviewText { get; set; }
    [Range(0, 10)] public int? Rating { get; set; }
    public bool? IsAnonymous { get; set; }
}