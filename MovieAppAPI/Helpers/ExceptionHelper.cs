using MovieAppAPI.Exceptions;

namespace MovieAppAPI.Helpers;

public static class ExceptionHelper {
    public static RecordNotFoundException UserNotFoundException(string? id = null, string? email = null,
        string? userName = null) {
        var idMessage = id is not null ? $" id \"{id}\" " : "";
        var emailMessage = email is not null ? $" email \"{email}\" " : "";
        var userNameMessage = userName is not null ? $" user name \"{userName}\" " : "";

        var message = idMessage + emailMessage + userNameMessage;

        return new RecordNotFoundException($"User with{message}not found");
    }

    public static AlreadyExistsException UserAlreadyExistsException(string? email = null, string? username = null) {
        if (email is null && username is null) {
            return new AlreadyExistsException("User with given email or user name already exists");
        }

        if (email is null && username is not null) {
            return new AlreadyExistsException($"User with user name \"{username}\" already exists");
        }

        if (email is not null && username is null) {
            return new AlreadyExistsException($"User with email \"{email}\" already exists");
        }

        return new AlreadyExistsException($"User with email \"{email}\" already exists");
    }

    public static ObjectsAreNotEqual PasswordsDoNotMatch() {
        return new ObjectsAreNotEqual("The entered passwords do not match");
    }
}