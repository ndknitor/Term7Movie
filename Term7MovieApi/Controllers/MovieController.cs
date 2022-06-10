using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieService.Services.Interface;
using System.Diagnostics;
using Term7MovieCore.Data.Request.Movie;

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
        public IActionResult GetMoviesForSpecificAction([FromQuery] MovieActionRequest request)
        {
            if (request.Action == "incoming")
                return GetIncomingMovies();
            if (request.Action == "latest")
                return GetEightLatestMovies();
            if (request.Action == "page")
            {
                return GetMoviesPaging(request.PageIndex);
                //return await GetAllMovie(pfr);
            }

            //if (request.Action == "detail")
            //    return GetMovieDetailById(request.movieId);
            return BadRequest(new ParentResponse { Message = "Quăng nó 404 đê" });
        }

        [HttpGet("{id:int}")]
        public IActionResult GetMoviesDetailFromID(int id)
        {
            var result = _movieService.FakeDetailMovieFor69();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateRequest[] request)
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                //thế là chúng ta có 2 version ez pz
                //var response = await _movieService.CreateMovieWithoutBusinessLogic(request);
                stopwatch.Start();
                var response = await _movieService.CreateMovie(request);
                stopwatch.Stop();
                //_logger.LogInformation("duration: " + stopwatch.ElapsedMilliseconds);
                //return Ok(response);
                return Ok(new ParentResponse { Message = "Cost " + stopwatch.ElapsedMilliseconds + " mili seconds for this action." });
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMovie(MovieUpdateRequest request)
        {
            try
            {
                Stopwatch stopwatch = new Stopwatch();
                //_logger.LogInformation(string.Join("", request.CategoryIDs));
                stopwatch.Start();
                var result = await _movieService.UpdateMovie(request);
                stopwatch.Stop();
                //return Ok(result);
                return Ok(new ParentResponse { Message = "Cost " + stopwatch.ElapsedMilliseconds + " mili seconds for this action." });
            }
            catch(Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(new ParentResponse { Message = "Chụp hình gửi Nam Trần nha huhu. " + ex.Message });
            }
        }

        /* ---------------- START PRIVATE METHODS ----------------- */
        //I have used too many brain cell for this lol if i get it wrong then sorry
        private IActionResult GetIncomingMovies()
        {
            var result = _movieService.FakeIncomingMovie();
            return Ok(result);
        }

        private IActionResult GetEightLatestMovies()
        {
            var result = _movieService.FakeShowingMovie();
            return Ok(result);
        }

        private IActionResult GetMoviesPaging(int pageIndex)
        {
            return Ok(new ParentResponse { Message = "bảo trì hệ thúm" });
        }

        /* ------------ END PRIVATE METHODS --------------- */
    }
}
