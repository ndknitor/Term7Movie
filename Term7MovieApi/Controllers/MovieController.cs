using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieService.Services.Interface;
using Term7MovieRepository.Repositories.Interfaces;
using System.Text.Json;

namespace Term7MovieApi.Controllers
{
    [Route("api/movie")]
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
        public async Task<IActionResult> GetAllMovie()
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
            return Ok();
        }

        [NonAuthorized]
        [HttpGet("get-less-detail-movie")]
        public async Task<IActionResult> GetThreeLattestMovies()
        {
            //sr vì chưa handle lỗi tốt lắm. hmu hmu
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
    }
}
