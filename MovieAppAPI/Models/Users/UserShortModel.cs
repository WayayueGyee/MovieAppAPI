namespace MovieAppAPI.Models.Users;

public class UserShortModel {
    public UserShortModel() {
        Id = "";
    }

    public UserShortModel(string id, string userName, string avatar) {
        Id = id;
        UserName = userName;
        Avatar = avatar;
    }

    public string Id { get; set; }
    public string? UserName { get; set; }
    public string? Avatar { get; set; }
}