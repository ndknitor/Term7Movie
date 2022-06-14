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
                return await GetMoviesPaging(new MovieListPageRequest 
                { PageIndex = request.PageIndex, PageSize = request.PageSize,
                    TitleSearch = request.SearchKey});
            }
            if (request.Action == "detail")
                return await GetMoviesDetailFromID(request.MovieId);
            if (request.Action == "titles")
                return await GetMovieTitles();
            return BadRequest(new ParentResponse { Message = "Quăng nó 404 đê" });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMoviesDetailFromID(int id)
        {
            var result = await _movieService.GetMovieDetailFromMovieId(id);
            return Ok(result);
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest[] request)
        {
            try
            {
                //Stopwatch stopwatch = Stopwatch.StartNew();
                //thế là chúng ta có 2 version ez pz
                //var response = await _movieService.CreateMovieWithoutBusinessLogic(request);
                //stopwatch.Start();
                var response = await _movieService.CreateMovie(request);
                //stopwatch.Stop();
                //_logger.LogInformation("duration: " + stopwatch.ElapsedMilliseconds);
                return Ok(response);
                //return Ok(new ParentResponse { Message = "Cost " + stopwatch.ElapsedMilliseconds + " mili seconds for this action." });
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpPut]
        public async Task<IActionResult> UpdateMovie(MovieUpdateRequest request)
        {
            try
            {
                //Stopwatch stopwatch = new Stopwatch();
                //_logger.LogInformation(string.Join("", request.CategoryIDs));
                //stopwatch.Start();
                var result = await _movieService.UpdateMovie(request);
                //stopwatch.Stop();
                return Ok(result);
                //return Ok(new ParentResponse { Message = "Cost " + stopwatch.ElapsedMilliseconds + " mili seconds for this action." });
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }

        [Authorize(Roles = Constants.ROLE_ADMIN)]
        [HttpDelete]
        public async Task<IActionResult> DeleteMovie(int movieid)
        {
            try
            {
                var result = await _movieService.DeleteMovie(movieid);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
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
        /* ------------ END PRIVATE METHODS --------------- */
    }
}
