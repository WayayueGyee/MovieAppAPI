namespace MovieAppAPI.Models.Users;

public class UserShortModel {
    public UserShortModel(string id, string nickName, string avatar) {
        Id = id;
        NickName = nickName;
        Avatar = avatar;
    }

    public string Id { get; set; }
    public string NickName { get; set; }
    public string Avatar { get; set; }
}