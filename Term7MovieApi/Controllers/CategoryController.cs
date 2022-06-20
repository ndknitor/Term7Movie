using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Term7MovieCore.Data.Response;
using Term7MovieService.Services.Interface;

namespace Term7MovieApi.Controllers
{
    [Route("api/v1/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly ICategoryService _cateService;
        //private readonly IUnitOfWork _unitOfWork;

        public CategoryController(ILogger<MovieController> logger, ICategoryService cateService)
        {
            _logger = logger;
            _cateService = cateService;

        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _cateService.GetFullCategory();
            return Ok(result);
        }
    }
}
