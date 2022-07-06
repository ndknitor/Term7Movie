using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Request.Movie;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Response.Movie;

namespace Term7MovieService.Services.Interface
{
    public interface IMovieService
    {
        Task<MovieListResponse> GetAllMovie(MovieFilterRequest request);

        Task<ParentResultResponse> GetMovieById(int id);
        
        Task<IncomingMovieResponse> GetThreeLosslessLatestMovieForHomepage();

        Task<MovieLatestResponse> GetLatestMovieForNowShowingPage(ParentFilterRequest request);


        Task<MoviePagingResponse> GetMovieListFollowPage(MovieListPageRequest mlpr);
        
        Task<MovieDetailResponse> GetMovieDetailFromMovieId(int movieId);

        Task<ParentResponse> CreateMovieWithoutBusinessLogic(MovieCreateRequest[] requests);

        Task<MovieCreateResponse> CreateMovie(MovieCreateRequest[] requests);

        Task<ParentResponse> UpdateMovie(MovieUpdateRequest request);

        Task<ParentResponse> DeleteMovie(int movieId);

        Task<MovieTitleResponse> GetMovieTitle();

        //Saved for some day that doesn't exists :v
        //Task<MovieHomePageResponse> GetMovieForHomePage(MovieHomePageRequest request);

        //optimize
        Task<MovieHomePageResponse> GetMovieRecommendationForHomePage(MovieHomePageRequest request);

        IncomingMovieResponse FakeIncomingMovie();

        MovieHomePageResponse FakeShowingMovie();

        MovieDetailResponse FakeDetailMovieFor69(int movieId = 69);
    }
}
