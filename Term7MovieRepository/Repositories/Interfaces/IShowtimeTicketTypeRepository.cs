using Term7MovieCore.Data.Dto;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IShowtimeTicketTypeRepository
    {
        Task<IEnumerable<ShowtimeTicketTypeDto>> GetShowtimeTicketTypeByShowtimeId(long showtimeId);
    }
}
