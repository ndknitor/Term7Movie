using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IMovieService
    {
        Task<MovieListResponse> GetAllMovie(ParentFilterRequest request);
        Task<IncomingMovieResponse> GetEightLosslessLatestMovieForHomepage();
        Task<MovieHomePageResponse> GetEightLatestMovieForHomepage();
        Task<TemptMoviePagingResponse> GetMovieListFollowPage(MovieListPageRequest mlpr);
        Task<MovieDetailResponse> GetMovieDetailFromMovieId(int movieId);
    }
}
