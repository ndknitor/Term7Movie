﻿using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Response.Movie;

namespace Term7MovieService.Services.Interface
{
    public interface IMovieService
    {
        Task<MovieListResponse> GetAllMovie(ParentFilterRequest request);
        
        Task<IncomingMovieResponse> GetEightLosslessLatestMovieForHomepage();
        
        Task<MovieHomePageResponse> GetEightLatestMovieForHomepage();
        
        Task<TemptMoviePagingResponse> GetMovieListFollowPage(MovieListPageRequest mlpr);
        
        Task<MovieDetailResponse> GetMovieDetailFromMovieId(int movieId);

        Task<ParentResponse> CreateMovieWithoutBusinessLogic(MovieCreateRequest[] requests);

        Task<MovieCreateResponse> CreateMovie(MovieCreateRequest[] requests);
    }
}
