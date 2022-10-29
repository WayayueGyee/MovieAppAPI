using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities;

namespace MovieAppAPI.Data; 

public class MovieDataContext : DbContext {
    public MovieDataContext(DbContextOptions<MovieDataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = default!;
}