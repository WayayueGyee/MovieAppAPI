using System.ComponentModel.DataAnnotations;

namespace MovieAppAPI.Models.Reviews;

public class ReviewShortModel {
    public Guid Id { get; set; }
    [Range(0, 10)] public int Rating { get; set; }
}