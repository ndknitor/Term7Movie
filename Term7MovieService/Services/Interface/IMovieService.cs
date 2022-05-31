﻿using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IMovieService
    {
        Task<MovieHomePageResponse> GetEightLosslessLatestMovieForHomepage();
        Task<MovieHomePageResponse> GetEightLatestMovieForHomepage();
    }
}
