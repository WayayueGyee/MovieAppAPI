namespace MovieAppAPI.Models.Users;

public class UserShortModel {
    public UserShortModel() {
        Id = Guid.Empty;
    }

    public UserShortModel(Guid id, string userName, string avatar) {
        Id = id;
        UserName = userName;
        Avatar = avatar;
    }

    public Guid Id { get; set; }
    public string? UserName { get; set; }
    public string? Avatar { get; set; }
}