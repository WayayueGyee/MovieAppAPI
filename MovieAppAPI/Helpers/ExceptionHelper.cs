using Microsoft.AspNetCore.Mvc;
using MovieAppAPI.Entities.Users;
using MovieAppAPI.Exceptions;

namespace MovieAppAPI.Helpers;

public static class ExceptionHelper {
    public static InvalidTokenException InvalidTokenException(string? message = null) {
        throw new InvalidTokenException(message);
    }

    public static RecordNotFoundException UserNotFoundException(string? id = null, string? email = null,
        string? userName = null) {
        var idMessage = id is not null ? $"id '{id}'" : "";
        var emailMessage = email is not null ? $"email '{email}'" : "";
        var userNameMessage = userName is not null ? $"user name '{userName}'" : "";
        var arr = new[] { idMessage, emailMessage, userNameMessage };

        var message = string.Join(" and ", arr.Where(s => !string.IsNullOrEmpty(s)));

        return new RecordNotFoundException($"User with{message}not found");
    }

    public static AlreadyExistsException UserAlreadyExistsException(string? email = null, string? username = null) {
        if (email is null && username is null) {
            return new AlreadyExistsException("User with given email or user name already exists");
        }

        if (email is null && username is not null) {
            return new AlreadyExistsException($"User with user name '{username}' already exists");
        }

        if (email is not null && username is null) {
            return new AlreadyExistsException($"User with email '{email}' already exists");
        }

        return new AlreadyExistsException($"User with email '{email}' or username '{username}' already exists");
    }

    public static ObjectsAreNotEqual PasswordsDoNotMatch() {
        return new ObjectsAreNotEqual("The entered passwords do not match");
    }

    public static AlreadyExistsException MovieAlreadyExistsException(string id) {
        return new AlreadyExistsException($"Movie with id '{id}' is already exists");
    }

    public static AlreadyExistsException FavoriteMovieAlreadyExistsException(string movieId, string userId) {
        return new AlreadyExistsException(
            $"Movie with id '{movieId}' has already been added to the user with id '{userId}'");
    }

    public static RecordNotFoundException FavoriteMovieNotFoundException(string movieId, string userId) {
        return new RecordNotFoundException(
            $"User with id '{userId}' doesn't have movie with id '{movieId}'");
    }

    public static RecordNotFoundException MovieNotFoundException(string id) {
        return new RecordNotFoundException($"Movie with id '{id}' not found");
    }

    public static AlreadyExistsException CountryAlreadyExistsException(string countryName) {
        return new AlreadyExistsException($"Country with name '{countryName}' is already exists");
    }

    public static RecordNotFoundException CountryNotFoundException(string? id = null, string? name = null) {
        var idMessage = id is null ? $"id '{id}'" : "";
        var nameMessage = name is null ? $"name '{name}'" : "";
        var arr = new[] { idMessage, nameMessage };

        var message = string.Join(" and ", arr.Where(s => !string.IsNullOrEmpty(s)));

        return new RecordNotFoundException($"Country with {message} not found");
    }

    public static RecordNotFoundException ReviewNotFoundException(string? id = null, string? movieId = null,
        string? userId = null) {
        var idMessage = id is null ? $"id '{id}'" : "";
        var movieIdMessage = movieId is null ? $"movie id '{movieId}'" : "";
        var userIdMessage = userId is null ? $"user id '{userId}'" : "";
        var arr = new[] { idMessage, movieIdMessage, userIdMessage };

        var message = string.Join(" and ", arr.Where(s => !string.IsNullOrEmpty(s)));

        return new RecordNotFoundException($"Review with {message} not found");
    }

    public static PermissionsDeniedException PermissionsDeniedException() {
        return new PermissionsDeniedException();
    }
}