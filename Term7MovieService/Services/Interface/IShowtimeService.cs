using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IShowtimeService
    {
        Task<ShowtimeListResponse> GetShowtimesAsync(ShowtimeFilterRequest request, long userId);
        Task<ShowtimeResponse> GetShowtimeByIdAsync(long id);
        Task<ShowtimeCreateResponse> CreateShowtimeAsync(ShowtimeCreateRequest request, long managerId);
        Task<ParentResponse> UpdateShowtimeAsync(ShowtimeUpdateRequest request);
    }
}
