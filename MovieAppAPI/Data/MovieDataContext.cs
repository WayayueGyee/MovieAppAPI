using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Auth;

namespace MovieAppAPI.Data; 

public class MovieDataContext : DbContext {
    public MovieDataContext(DbContextOptions<MovieDataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<ValidToken> ValidTokens { get; set; } = default!;
}