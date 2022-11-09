using MovieAppAPI.Models.Pagination;

namespace MovieAppAPI.Models.Movies;

public class MoviePagedListModel {
    public MoviePagedListModel(PageInfoModel pageInfo, List<MovieElementModel>? movies = null) {
        PageInfo = pageInfo;
        Movies = movies;
    }

    public List<MovieElementModel>? Movies { get; set; }
    public PageInfoModel PageInfo { get; set; }
}