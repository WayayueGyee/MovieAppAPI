namespace MovieAppAPI.Exceptions; 

public class PermissionsDeniedException : Exception {
    public PermissionsDeniedException() { }
    public PermissionsDeniedException(string? message) : base(message) { }
    public PermissionsDeniedException(string? message, Exception? innerException) : base(message, innerException) { }
}