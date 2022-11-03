using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Models.Users;

namespace MovieAppAPI.Entities; 

[Table("review")]
public class Review {
    [Key] public Guid Id { get; set; }
    public Movie Movie { get; set; }
    public User User { get; set; }
    public int Rating { get; set; }
    public string ReviewText { get; set; }
    public bool IsAnonymous { get; set; }
    public DateTime CreateDateTime { get; set; }
    public UserShortModel Author { get; set; }
}   