using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data;
using Term7MovieCore.Entities;

namespace Term7MovieApi.Controllers
{
    [Route("api/movie")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [Authorize(Roles = Constants.ROLE_CUSTOMER)]
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
            return Ok(movie);
        }
    }
}
