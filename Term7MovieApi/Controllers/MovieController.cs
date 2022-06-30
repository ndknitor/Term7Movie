using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieService.Services.Interface;
using System.Diagnostics;
using Term7MovieCore.Data.Request.Movie;
using Term7MovieCore.Data;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _movieService;
        //private readonly IUnitOfWork _unitOfWork;

        public MovieController(ILogger<MovieController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;

        }

        [HttpGet]
        private async Task<IActionResult> GetAllMovie([FromQuery] ParentFilterRequest request)
        {
            var response = await _movieService.GetAllMovie(request);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMoviesForSpecificAction([FromQuery] MovieActionRequest request)
        {
            switch(request.Action)
            {
                case "page":
                    return await GetAllMovie(new ParentFilterRequest { Page = request.PageIndex, PageSize = request.PageSize, SearchKey = request.SearchKey});
                case "home":
                    return await GetMoviesForMobileHome(new MovieHomePageRequest { Latitude = request.Latitude, Longtitude = request.Longitude });
                case "incoming":
                    return await GetIncomingMovies();
                case "latest":
                    return await GetEightLatestMovies();
                case "undermaintain-page":
                    return await GetMoviesPaging(new MovieListPageRequest
                    {
                        PageIndex = request.PageIndex,
                        PageSize = request.PageSize,
                        TitleSearch = request.SearchKey
                    });
                case "detail":
                    return await GetMoviesDetailFromID(request.MovieId);
                case "titles":
                        return await GetMovieTitles();
                default:
                    return BadRequest(new ParentResponse { Message = "Quăng nó 404 đê" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMoviesDetailFromID(int id)
        {
            var result = await _movieService.GetMovieById(id);
            return Ok(result);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest[] request)
        {
            var response = await _movieService.CreateMovie(request);
            return Ok(response);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdateMovie(MovieUpdateRequest request)
        {

            var result = await _movieService.UpdateMovie(request);
            return Ok(result);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(int movieid)
        {
                var result = await _movieService.DeleteMovie(movieid);
                return Ok(result);
        }

        /* ---------------- START PRIVATE METHODS ----------------- */
        //I have used too many brain cell for this lol if i get it wrong then sorry
        private async Task<IActionResult> GetIncomingMovies()
        {
            var result = await _movieService.GetThreeLosslessLatestMovieForHomepage();
            if (result == null)
                return BadRequest(new ParentResponse { Message = "NULL DATA" });
            return Ok(result);
        }

        private async Task<IActionResult> GetEightLatestMovies()
        {
            try
            {
                var result = await _movieService.GetEightLatestMovieForHomepage();
                if (result == null)
                    return BadRequest(new ParentResponse { Message = "NULL DATA" });
                return Ok(result);
            }
            catch
            {
                return BadRequest(new ParentResponse { Message = "CANNOT REACH DATABASE" });
            }
        }

        private async Task<IActionResult> GetMoviesPaging(MovieListPageRequest request)
        {
            var result = await _movieService.GetMovieListFollowPage(request);
            return Ok(result);
        }

        private async Task<IActionResult> GetMovieTitles()
        {
            var result = await _movieService.GetMovieTitle();
            return Ok(result);
        }

        private async Task<IActionResult> GetMoviesForMobileHome(MovieHomePageRequest request)
        {
            if (request.Longtitude == 0 && request.Latitude == 0)
                return BadRequest(new ParentResponse { Message = "Đang giữa biển châu phi à?" });
            var result = await _movieService.GetMovieRecommendationForHomePage(request);
            return Ok(result);
        }
        /* ------------ END PRIVATE METHODS --------------- */
    }
}
