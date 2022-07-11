using Microsoft.AspNetCore.Http;
using Term7MovieCore.Data.Dto;

namespace Term7MovieService.Services.Interface
{
    public interface IImageHostService
    {
        Task<ImageBBResponseData> UploadImageAsync(IFormFile image, string companyName);
    }
}
