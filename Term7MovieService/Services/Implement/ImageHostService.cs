using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Text;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Extensions;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class ImageHostService : IImageHostService
    {
        private readonly ImgBBOption _imgBBOption;

        private const string IMAGE_PARAM = "image";

        public ImageHostService(IOptions<ImgBBOption> option)
        {
            _imgBBOption = option.Value;
        }

        public async Task<ImageBBResponseData> UploadImageAsync(IFormFile image, string companyName)
        {
            ImageBBResponse imgResponse;
            if (image.Length == 0) return null;

            using(HttpClient client = new HttpClient())
            {
                string url = $"{_imgBBOption.UploadUrl}?expiration={_imgBBOption.Expiration}&key={_imgBBOption.APIKey}";

                MemoryStream stream = new MemoryStream();

                await image.CopyToAsync(stream);

                byte[] bytes = stream.ToArray();

                ByteArrayContent imgContent = new ByteArrayContent(bytes);

                MultipartFormDataContent form = new MultipartFormDataContent();

                form.Add(imgContent, IMAGE_PARAM, companyName.ToSnakeCase() + "_" + Guid.NewGuid());

                var response = await client.PostAsync(url, form);

                if (response.IsSuccessStatusCode)
                {
                    string message = await response.Content.ReadAsStringAsync();

                    imgResponse = message.ToObject<ImageBBResponse>();

                    if (imgResponse != null) return imgResponse.Data;
                }
            }

            return null;
        }
    }
}
