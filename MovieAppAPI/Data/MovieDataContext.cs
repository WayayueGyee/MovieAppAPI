using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Entities.Users;

namespace MovieAppAPI.Data;

public class MovieDataContext : DbContext {
    public MovieDataContext(DbContextOptions<MovieDataContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<InvalidToken> ValidTokens { get; set; } = default!;
}