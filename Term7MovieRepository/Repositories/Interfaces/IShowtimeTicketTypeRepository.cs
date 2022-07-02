using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IShowtimeTicketTypeRepository
    {
        Task<IEnumerable<ShowtimeTicketTypeDto>> GetShowtimeTicketTypeByShowtimeId(long showtimeId);
        Task<int> InsertShowtimeTicketType(ShowtimeTicketTypeCreateRequest request);
    }
}
