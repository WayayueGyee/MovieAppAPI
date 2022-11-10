using System.Diagnostics;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MovieAppAPI.Config;
using MovieAppAPI.Data;
using MovieAppAPI.Middlewares;
using MovieAppAPI.Services.Auth;
using MovieAppAPI.Services.Countries;
using MovieAppAPI.Services.FavouriteMovies;
using MovieAppAPI.Services.Movies;
using MovieAppAPI.Services.Reviews;
using MovieAppAPI.Services.Users;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

Assembly assembly = Assembly.GetExecutingAssembly();
FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
Console.WriteLine(fileVersionInfo.ProductVersion);


services.AddDbContext<MovieDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ??
                      throw new InvalidOperationException("Connection string 'DataContext' not found.")));

// Add services to the container.
services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    // options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// AutoMapper
// services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var config = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
var mapper = config.CreateMapper();
services.AddSingleton(mapper);

// Authentication
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
    var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

    if (isDevelopment) {
        options.RequireHttpsMetadata = false;
    }

    options.TokenValidationParameters = TokenConfig.CreateValidationParameters();
});

// Authorization
services.AddAuthorization()
    .AddScoped<IAuthorizationHandler, TokenNotRejectedAuthorizationHandler>()
    .AddAuthorization(options => {
        options.AddPolicy("TokenNotRejected",
            policyBuilder =>
                policyBuilder
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .AddRequirements(new TokenNotRejectedRequirement()));
    });

// Configure DI
services.AddScoped<IUserService, UserService>();
services.AddScoped<IAuthService, AuthService>();
services.AddScoped<ITokenService, TokenService>();
services.AddScoped<IMovieService, MovieService>();
services.AddScoped<ICountryService, CountryService>();
services.AddScoped<IReviewService, ReviewService>();
services.AddScoped<IFavoriteMovieService, FavoriteMovieService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option => {
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieCatalogApi", Version = "v1" });
    option.MapType<TimeSpan?>(() => new OpenApiSchema { Type = "string", Example = new OpenApiString("00:00:00") });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.Run();