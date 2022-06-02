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


        //[Authorize(Roles = Constants.ROLE_CUSTOMER)]
        [HttpGet("all")]
        public IActionResult GetAllMovie()
        {
            var movie = new Movie[]
            {
                new Movie
                {
                    Id = 1,
                    CoverImageUrl = "google.com",
                    Description = "Test",
                    ReleaseDate = DateTime.UtcNow,
                    Title = "Test Movie 1"
                },
                new Movie
                {
                    Id = 2,
                    CoverImageUrl = "google.com",
                    Description = "Test",
                    ReleaseDate = DateTime.UtcNow,
                    Title = "Test Movie 2"
                }
            };
            //foreach (var item in await _movieService.GetThreeLatestMovieForHomepage())
            //{
            //    _logger.LogInformation(item.Message + "_" + item.movieID + "_" + item.coverImgURL);
            //}
            return Ok(movie);
        }

        [AllowAnonymous]
        [HttpGet("movie-incoming")]
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
        [HttpGet("movie-now-showing")]
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
