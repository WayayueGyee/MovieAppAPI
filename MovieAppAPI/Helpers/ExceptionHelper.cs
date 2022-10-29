using MovieAppAPI.Exceptions;

namespace MovieAppAPI.Helpers; 

public static class ExceptionHelper {
    public static RecordNotFoundException UserNotFoundException(string id) {
        return new RecordNotFoundException("User with id: " + id + " not found");
    }

    public static AlreadyExistsException UserAlreadyExistsException() {
        return new AlreadyExistsException("User with given email or user name already exists");
    }
}