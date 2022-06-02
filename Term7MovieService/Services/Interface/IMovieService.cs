using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;

namespace Term7MovieService.Services.Interface
{
    public interface IMovieService
    {
        Task<IncomingMovieResponse> GetEightLosslessLatestMovieForHomepage();
        Task<MovieHomePageResponse> GetEightLatestMovieForHomepage();
        Task<MovieListResponse> GetMovieListFollowPage(MovieListPageRequest request);
    }
}
