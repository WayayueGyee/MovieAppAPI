using System.Diagnostics;
using System.Text;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Data;
using MovieAppAPI.Entities;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Helpers;
using MovieAppAPI.Models.Movies;
using MovieAppAPI.Models.Reviews;
using MovieAppAPI.Models.Users;

namespace DatabaseFill;

internal static class Program {
    private static readonly HttpClient Client = new();
    private const string ApiUrl = "https://react-midterm.kreosoft.space/api";

    private static async Task<IEnumerable<MovieDetailsModel>> FetchMovieDetails() {
        var movies = new HashSet<MovieDetailsModel>();
        int page = 1, pageCount = 10;
        do {
            MoviePagedListModel? res = null;
            try {
                res = await Client.GetAsync($"{ApiUrl}/movies/{page++}")
                    .Result
                    .Content
                    .ReadFromJsonAsync<MoviePagedListModel>();
            }
            catch (Exception ex) {
                Debug.WriteLine($"{ex.Message}; \n Occured when fetching page : {page}");
            }

            if (res is null) continue;
            pageCount = res.PageInfo.TotalPages;

            foreach (var movie in res.Movies!) {
                MovieDetailsModel? movieDetails = null;
                try {
                    movieDetails = await Client.GetAsync($"{ApiUrl}/movies/details/{movie.Id}")
                        .Result
                        .Content
                        .ReadFromJsonAsync<MovieDetailsModel>();
                }
                catch (Exception ex) {
                    Debug.WriteLine($"{ex.Message}; \nOccured when fetching movie id : {movie.Id}");
                }

                if (movieDetails is null) continue;

                movieDetails.Reviews = movieDetails.Reviews?
                    .DistinctBy(md => md.Id).ToList() ?? new List<ReviewModel>();

                for (var i = 0; i < movieDetails.Reviews.Count; i++) {
                    movieDetails.Reviews[i].CreateDateTime = movieDetails.Reviews[i].CreateDateTime.ToUniversalTime();

                    if (movieDetails.Reviews[i].Author == null) {
                        movieDetails.Reviews[i].Author = new UserShortModel
                            { Id = Guid.NewGuid(), UserName = GenerateRandomString(), Avatar = "" };
                        continue;
                    }

                    movieDetails.Reviews[i].Author!.Id = Guid.NewGuid();
                }

                movieDetails.Genres = movieDetails.Genres?.DistinctBy(g => g.Id).ToList();

                movies.Add(movieDetails);
            }
        } while (page <= pageCount);

        return movies.DistinctBy(md => md.Id);
    }

    private static Dictionary<Guid, List<Genre>> ExtractGenres(IEnumerable<Movie> movies) {
        var moviesList = movies as List<Movie> ?? movies.ToList();
        var movieGenres = new Dictionary<Guid, List<Genre>>(moviesList.Count);

        foreach (var movie in moviesList) {
            if (movie.Genres == null) continue;
            movieGenres.Add(movie.Id, movie.Genres.DistinctBy(g => g.Id).ToList());
            movie.Genres = null;
        }

        return movieGenres;
    }

    private static IEnumerable<Review> ExtractReviews(IEnumerable<Movie> movies) {
        var reviews = new List<Review>();
        foreach (var movie in movies) {
            if (movie.Reviews != null) reviews.AddRange(movie.Reviews);
        }

        var uniqueReviews = reviews.DistinctBy(r => r.Id).ToList();

        foreach (var uniqueReview in uniqueReviews) {
            uniqueReview.UserId = uniqueReview.Author!.Id;
        }

        return uniqueReviews;
    }
    
    private static IEnumerable<User> ExtractUsers(IEnumerable<Review> reviews) {
        var authors = new HashSet<User>();

        foreach (var review in reviews) {
            if (review.Author is null) continue;
            
            authors.Add(new User {
                Avatar = review.Author.Avatar,
                Email = GenerateRandomString() + "@mail.com",
                Id = review.Author.Id,
                UserName = GenerateRandomString(),
                PasswordHash = HashingHelper.ComputeSha256Hash(review.Author.Id.ToString()),
                Name = GenerateRandomString()
            });
        }

        return authors.DistinctBy(user => user.Id);
    }

    private static IEnumerable<Country> ExtractCountries(IEnumerable<MovieDetailsModel> movieDetails) {
        var countries = new HashSet<Country>();

        foreach (var movieDetail in movieDetails) {
            if (movieDetail.Country is null) continue;

            countries.Add(new Country { Id = Guid.NewGuid(), CountryName = movieDetail.Country });
        }

        return countries.DistinctBy(c => c.CountryName);
    }

    private static string GenerateRandomString() {
        const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string numbersAndSymbols = "0123456789_-";
        const string availableSymbols = letters + numbersAndSymbols;

        var result = new StringBuilder();
        var random = new Random();
        var randomLetter = letters[random.Next(0, letters.Length)];
        result.Append(randomLetter);
        var length = random.Next(25, 55);

        for (var i = 0; i < length; i++) {
            result.Append(availableSymbols[random.Next(0, availableSymbols.Length)]);
        }

        return result.ToString();
    }

    private static async Task Main() {
        var movieDetails = await FetchMovieDetails();
        var movieDetailsModels = movieDetails as List<MovieDetailsModel> ?? movieDetails.ToList();
        var countries = ExtractCountries(movieDetailsModels);
        var movies = movieDetailsModels.Select(md => {
            var movie = (Movie)md;
            movie.CountryId =
                countries.SingleOrDefault(country => country.CountryName == movie.Country?.CountryName)?.Id ?? null;
            movie.Country = null;
            return movie;
        }).ToList();
        var movieGenres = ExtractGenres(movies);

        var genres = new HashSet<Genre>();
        foreach (var someGenres in movieGenres.Values) genres.UnionWith(someGenres);
        genres = genres.DistinctBy(genre => new { genre.Id, genre.Name }).ToHashSet();

        var reviews = ExtractReviews(movies).ToList();
        var users = ExtractUsers(reviews);

        foreach (var movie in movies) {
            movie.Genres = null;
            movie.Reviews = null;
        }

        foreach (var review in reviews) {
            review.Author = null;
        }
        
        var context = new MovieDataContext(new DbContextOptions<MovieDataContext>());

        context.Users.AddRange(users);
        await context.SaveChangesAsync();

        context.Genres.AddRange(genres);
        await context.SaveChangesAsync();

        context.Countries.AddRange(countries);
        await context.SaveChangesAsync();

        context.Movies.AddRange(movies);
        await context.SaveChangesAsync();

        context.Reviews.AddRange(reviews);
        await context.SaveChangesAsync();

        // adding relations Genre-Movie
        foreach (var pair in movieGenres) {
            var entity = await context.Movies.FindAsync(pair.Key);
            if (entity is null || pair.Value.Count == 0) continue;

            foreach (var genre in pair.Value)
                context.MovieGenres.Add(new MovieGenre { GenreId = genre.Id, MovieId = entity.Id });
        }

        await context.SaveChangesAsync();
    }
}