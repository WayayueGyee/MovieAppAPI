using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using MovieAppAPI.Config;
using MovieAppAPI.Data;
using MovieAppAPI.Middlewares;
using MovieAppAPI.Services.Auth;
using MovieAppAPI.Services.Users;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddDbContext<MovieDataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ??
                      throw new InvalidOperationException("Connection string 'DataContext' not found.")));

// Add services to the container.
services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

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