using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieAppAPI.Entities.Users;

namespace MovieAppAPI.Entities;

[Table("review")]
public class Review {
    [Key] public Guid Id { get; set; }
    [Required] public int Rating { get; set; }
    public string? ReviewText { get; set; }
    [Required] public bool IsAnonymous { get; set; }
    [Required] public DateTime CreateDateTime { get; set; }

    [Required] public Guid MovieId { get; set; }
    [ForeignKey("MovieId")] public Movie Movie { get; set; }

    [Required]
    [Column("Author")]
    public Guid UserId { get; set; }

    [ForeignKey("UserId")] public User Author { get; set; }
}