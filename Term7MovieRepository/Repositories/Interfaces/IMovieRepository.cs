﻿using Term7MovieCore.Entities;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto.Errors;
using Term7MovieCore.Data.Request.Movie;
using Term7MovieCore.Data.Dto.Movie;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IMovieRepository
    {
        Task<PagingList<MovieModelDto>> GetAllMovie(MovieFilterRequest request);
        Task<MovieModelDto> GetMovieByIdAsync(int id);
        IEnumerable<MovieModelDto> GetAllMovie();
        IEnumerable<Movie> MovieEntityToList();
        Task<Movie> GetMovieById(int id);
        Task<bool> CreateMovie(IEnumerable<Movie> movie);
        Task<bool> UpdateMovie(Movie movie);
        Task DeleteMovie(int movieId);
        int Count();
        Task<IEnumerable<SmallMovieHomePageDTO>> GetLessThanThreeLosslessLatestMovies();
        Task<IEnumerable<Movie>> GetEightLatestMovies();
        Task<Dictionary<int, IEnumerable<MovieType>>> GetCategoriesFromMovieList(int[] MovieIds);
        Task<IEnumerable<Movie>> GetMoviesFromSpecificPage(int page, int pageCapacity, string searchKey);
        Task<IEnumerable<MovieType>> GetCategoryFromSpecificMovieId(int movieId);
        Task<CreateMovieError> CreateMovieWithCategory(MovieCreateRequest request);
        Task<bool> UpdateMovie(MovieUpdateRequest request);
        Task<IEnumerable<Movie>> GetMoviesTitle();
        Task<IEnumerable<Movie>> GetRemainInformationForHomePage(int[] MovieIds);
    }
}
