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
        public async Task<IActionResult> GetMoviesForSpecificAction([FromQuery] MovieActionRequest request)
        {
            if (request.Action == "incoming")
                return await GetIncomingMovies();
            if (request.Action == "latest")
                return await GetEightLatestMovies();
            if (request.Action == "page")
            {
                ParentFilterRequest pfr = new ParentFilterRequest
                {
                    Page = request.PageIndex,
                    PageSize = request.PageSize,
                    SearchKey = request.SearchKey
                };
                return await GetAllMovie(pfr);
                //return await GetAllMovie(pfr);
            }

            if (request.Action == "detail")
                return Ok(new ParentResponse { Message = "Do thầy đòi đẩy ra 1 api nên là cái này khum dùng nữa :v dùng cái dưới i" });
            return BadRequest(new ParentResponse { Message = "Quăng nó 404 đê" });
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetMoviesDetailFromID(int id)
        {
            var result = await _movieService.GetMovieDetailFromMovieId(id);
            return Ok(result);
        }

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
            var result = _movieService.FakeIncomingMovie();
            return Ok(result);
        }

        private async Task<IActionResult> GetEightLatestMovies()
        {
            var result = _movieService.FakeShowingMovie();
            return Ok(result);
        }

        private async Task<IActionResult> GetMoviesPaging(int pageIndex)
        {
            return Ok(new ParentResponse { Message = "bảo trì hệ thúm" });
        }

        /* ------------ END PRIVATE METHODS --------------- */
    }
}
