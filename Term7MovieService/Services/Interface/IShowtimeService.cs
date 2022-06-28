using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IShowtimeService
    {
        Task<ShowtimeListResponse> GetShowtimesAsync(ShowtimeFilterRequest request, long userId, string roleName);
        Task<ShowtimeResponse> GetShowtimeByIdAsync(long id);
        Task<ShowtimeCreateResponse> CreateShowtimeAsync(ShowtimeCreateRequest request);
        Task<ParentResponse> UpdateShowtimeAsync(ShowtimeUpdateRequest request);
    }
}
