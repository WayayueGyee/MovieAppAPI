namespace MovieAppAPI.Exceptions; 

public class ObjectsAreNotEqual : Exception {
    public ObjectsAreNotEqual() { }
    public ObjectsAreNotEqual(string? message) : base(message) { }
    public ObjectsAreNotEqual(string? message, Exception? innerException) : base(message, innerException) { }
}