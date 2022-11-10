using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Entities;

[Table("review")]
[Index("MovieId", "UserId", IsUnique = true)]
public class Review {
    public Review() {
        Id = Guid.Empty;
    }

    public Review(Guid id) {
        Id = id;
    }

    [Key] public Guid Id { get; set; }
    [Range(0, 10)] [Required] public int Rating { get; set; }
    public string? ReviewText { get; set; }
    [Required] public bool IsAnonymous { get; set; }
    [Required] public DateTime CreateDateTime { get; set; }

    [Required] public Guid MovieId { get; set; }
    [ForeignKey("MovieId")] public Movie Movie { get; set; }

    [Required] [Column("Author")] public Guid? UserId { get; set; }
    [ForeignKey("UserId")] public User? Author { get; set; }

    public static explicit operator Review(ReviewModel reviewModel) {
        return new Review {
            Id = reviewModel.Id,
            Author = reviewModel.Author is null ? null : (User)reviewModel.Author,
            Rating = reviewModel.Rating,
            IsAnonymous = reviewModel.IsAnonymous,
            ReviewText = reviewModel.ReviewText,
            CreateDateTime = reviewModel.CreateDateTime
        };
    }
}