using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieService.Services.Interface;
using Term7MovieRepository.Repositories.Interfaces;
using System.Text.Json;
using System.Diagnostics;
using Term7MovieCore.Data.Request;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1")]
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

        [HttpPost("all")]
        public async Task<IActionResult> GetAllMovie(ParentFilterRequest request)
        {
            var response = await _movieService.GetAllMovie(request);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpGet("movies/incoming")]
        public async Task<IActionResult> GetIncomingMovies()
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

        [AllowAnonymous]
        [HttpGet("movies/latest")]
        public async Task<IActionResult> GetEightLatestMovies()
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

        [AllowAnonymous]
        [HttpGet("movie/{PageIndex:int}")]
        public async Task<IActionResult> GetMoviesPaging(int PageIndex)
        {
            try
            {
                MovieListPageRequest mlpr = new MovieListPageRequest(){PageIndex = PageIndex,};
                var result = await _movieService.GetMovieListFollowPage(mlpr);
                if (result == null)
                    return BadRequest(new ParentResponse { Message = "Singleton dead." });
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ParentResponse { Message = ex.Message });
            }
        }
    }
}
