using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ICategoryService
    {
        Task<CategoryResponse> GetFullCategory();
    }
}
