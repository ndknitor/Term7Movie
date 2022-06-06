using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieService.Services.Interface;
using System.Diagnostics;


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

        //lmao
        [HttpGet]//tôi mất 25p chỉ để hiểu rằng t sẽ làm thế này (:
        public async Task<IActionResult> GetMoviesForSpecificAction([FromQuery] MovieActionRequest request)
        {
            if (request.Action == "incoming")
                return await GetIncomingMovies();
            if (request.Action == "latest")
                return await GetEightLatestMovies();
            if (request.Action == "page")
            {
                ParentFilterRequest pfr = new ParentFilterRequest()
                {
                    Page = request.PageIndex,
                    PageSize = request.PageSize,
                    SearchKey = request.SearchKey
                };
                return await GetAllMovie(pfr);
            }
                
            if (request.Action == "detail")
                return await GetMovieDetailById(request.movieId);
            return BadRequest(new ParentResponse { Message = "Quăng nó 404 đê" });
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest[] request)
        {
            try
            {
                var response = await _movieService.CreateMovieWithoutBusinessLogic(request);
                return Ok(response);
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Business Logic went wrong." });
            }
        }

        /* ---------------- START PRIVATE METHODS ----------------- */
        //I have used too many brain cell for this lol if i get it wrong then sorry
        private async Task<IActionResult> GetIncomingMovies()
        {
            //chưa dùng đến. (để dự phòng thôi)
            try
            {
                var result = await _movieService.GetEightLosslessLatestMovieForHomepage();
                if (result == null)
                    return BadRequest(new ParentResponse { Message = "NULL DATA" });
                return Ok(result);
            }
            catch
            {
                return BadRequest(new ParentResponse { Message = "CANNOT REACH DATABASE" });
            }
        }

        private async Task<IActionResult> GetEightLatestMovies()
        {
            //sr vì chưa handle lỗi tốt lắm. hmu hmu
            try
            {
                Stopwatch zaWarudo = new Stopwatch();
                zaWarudo.Start();
                var result = await _movieService.GetEightLatestMovieForHomepage();
                zaWarudo.Stop();
                _logger.LogInformation("The power of Dio has stopped the world for: " + zaWarudo.ElapsedMilliseconds);
                if (result == null)
                    return BadRequest(new ParentResponse { Message = "NULL DATA" });
                return Ok(result);
            }
            catch
            {
                return BadRequest(new ParentResponse { Message = "CANNOT REACH DATABASE" });
            }
        }

        private async Task<IActionResult> GetMoviesPaging(int pageIndex)
        {
            _logger.LogInformation("here is the page index: " + pageIndex);
            try
            {
                MovieListPageRequest mlpr = new MovieListPageRequest() { PageIndex = pageIndex, };
                var result = await _movieService.GetMovieListFollowPage(mlpr);
                if (result == null)
                    return BadRequest(new ParentResponse { Message = "Singleton dead." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "oh sh*t not good" });
            }
        }


        private async Task<IActionResult> GetMovieDetailById(int movieId)
        {
            try
            {
                var result = await _movieService.GetMovieDetailFromMovieId(movieId);
                if (result == null)
                    return BadRequest(new ParentResponse { Message = "Singleton dead." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "oh sh*t not good" });
            }
        }

        /* ------------ END PRIVATE METHODS --------------- */
    }
}
