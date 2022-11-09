using System.Text.Json.Serialization;

namespace MovieAppAPI.Models.Pagination;

public class PageInfoModel {
    public int PageSize { get; set; }
    [JsonPropertyName("pageCount")] public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
}