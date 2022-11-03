using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAppAPI.Entities;

[Table("genre")]
public class Genre {
    // I don't want to use GUID here, it's just a task
    [Key] public Guid Id { get; set; }
    public string Name { get; set; }
    
    public List<Movie> Movies { get; set; }
}