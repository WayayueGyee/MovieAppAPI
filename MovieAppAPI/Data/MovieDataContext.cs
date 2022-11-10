using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Auth;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Pagination;

namespace MovieAppAPI.Data;

public class MovieDataContext : DbContext {
    public MovieDataContext(DbContextOptions<MovieDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        GuidExtension.Register(modelBuilder);

        modelBuilder.Entity<FavoriteMovie>()
            .HasKey(t => new { t.MovieId, t.UserId });
        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(fm => fm.Movie)
            .WithMany(m => m.FavoriteMovies)
            .HasForeignKey(fm => fm.MovieId);
        modelBuilder.Entity<FavoriteMovie>()
            .HasOne(fm => fm.User)
            .WithMany(u => u.FavoriteMovies)
            .HasForeignKey(fm => fm.UserId);

        modelBuilder.Entity<MovieGenre>()
            .HasKey(t => new { t.MovieId, t.GenreId });
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Movie)
            .WithMany(m => m.MovieGenres)
            .HasForeignKey(mg => mg.MovieId);
        modelBuilder.Entity<MovieGenre>()
            .HasOne(mg => mg.Genre)
            .WithMany(g => g.MovieGenres)
            .HasForeignKey(mg => mg.GenreId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (optionsBuilder.IsConfigured) return;

        optionsBuilder.EnableSensitiveDataLogging();
        
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetDirectoryRoot("/home/vladovello/VSCodeProjects/MovieAppAPI/MovieAppAPI/Program.cs"))
            .AddJsonFile("/home/vladovello/VSCodeProjects/MovieAppAPI/DatabaseFill/appsettings.json")
            .Build();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        optionsBuilder.UseNpgsql(connectionString);
    }

    public DbSet<User> Users { get; set; } = default!;
    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Movie> Movies { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Genre> Genres { get; set; } = default!;
    public DbSet<MovieGenre> MovieGenres { get; set; } = default!;
    public DbSet<FavoriteMovie> FavoriteMovies { get; set; } = default!;
    public DbSet<InvalidToken> ValidTokens { get; set; } = default!;
}