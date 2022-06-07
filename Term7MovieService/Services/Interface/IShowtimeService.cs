using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IShowtimeService
    {
        Task<ShowtimeListResponse> GetShowtimesByTheaterIdAsync(ShowtimeFilterRequest request);
        Task<ShowtimeResponse> GetShowtimeByIdAsync(long id);
        Task<ParentResponse> CreateShowtimeAsync(ShowtimeCreateRequest request, long managerId);
        Task<ParentResponse> UpdateShowtimeAsync(ShowtimeUpdateRequest request);
    }
}
