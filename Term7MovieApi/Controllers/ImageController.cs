using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageHostService _imageHostService;

        public ImageController(IImageHostService imageHostService)
        {
            _imageHostService = imageHostService;
        }

        [HttpPost]
        public async Task<IActionResult> GetImageBBResponse(IFormFile image)
        {
            string companyName = "Death Company";

            var response = await _imageHostService.UploadImageAsync(image, companyName);

            return Ok(response);
        }
    }
}
