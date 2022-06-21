using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IShowtimeTicketTypeService
    {
        Task<ParentResultResponse> GetShowtimeTicketTypeByShowtimeIdAsync(long showtimeId);
    }
}
