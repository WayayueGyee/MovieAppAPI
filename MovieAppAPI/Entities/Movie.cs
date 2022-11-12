using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Build.Evaluation;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Models.Reviews;

namespace MovieAppAPI.Entities;

[Table("movie")]
public class Movie {
    public Movie() {
        Id = Guid.NewGuid();
    }

    public Movie(Guid id) {
        Id = id;
    }

    public Movie(Guid id, string? name, string? poster, int year, int time, string? tagline, string? description,
        string? director, int? budget, int? fees, int ageLimit, Guid countryId, Country? country, List<Review> reviews,
        List<Genre> genres) {
        Id = id;
        Name = name;
        Poster = poster;
        Year = year;
        Time = time;
        Tagline = tagline;
        Description = description;
        Director = director;
        Budget = budget;
        Fees = fees;
        AgeLimit = ageLimit;
        CountryId = countryId;
        Country = country;
        Reviews = reviews;
        Genres = genres;
    }

    [Key] public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Poster { get; set; }
    [Required] public int Year { get; set; }
    [Required] public int Time { get; set; }
    public string? Tagline { get; set; }
    public string? Description { get; set; }
    public string? Director { get; set; }
    public int? Budget { get; set; }
    public int? Fees { get; set; }
    public int AgeLimit { get; set; }

    public Guid? CountryId { get; set; }
    public Country? Country { get; set; }

    public List<Review>? Reviews { get; set; }
    public List<Genre>? Genres { get; set; }

    public List<FavoriteMovie> FavoriteMovies { get; set; }
    public List<MovieGenre> MovieGenres { get; set; }

    public static explicit operator Movie(MovieDetailsModel movieDetailsModel) {
        List<Review>? review;
        var badReview = new ReviewModel();
        try {
            review = movieDetailsModel.Reviews?.Select(r => {
                badReview = r;
                var newReview = (Review)r;
                newReview.MovieId = movieDetailsModel.Id;
                return newReview;
            }).ToList();
        }
        catch (Exception e) {
            Console.WriteLine(badReview);
            throw;
        }

        return new Movie {
            Id = movieDetailsModel.Id,
            Name = movieDetailsModel.Name,
            Poster = movieDetailsModel.Poster,
            Year = movieDetailsModel.Year,
            Time = movieDetailsModel.Time,
            Tagline = movieDetailsModel.Tagline,
            Description = movieDetailsModel.Description,
            Director = movieDetailsModel.Director,
            Budget = movieDetailsModel.Budget,
            Fees = movieDetailsModel.Fees,
            AgeLimit = movieDetailsModel.AgeLimit,
            Country =
                movieDetailsModel.Country is null ? null : new Country { CountryName = movieDetailsModel.Country },
            Reviews = review,
            Genres = movieDetailsModel.Genres?.Select(genre => (Genre)genre).ToList()
        };
    }
}