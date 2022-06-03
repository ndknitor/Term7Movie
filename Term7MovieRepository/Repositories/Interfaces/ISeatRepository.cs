using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<SeatDto>> GetAllSeat(int roomId);
        Task<SeatDto> GetSeatById(long id);
        int CreateSeat(Seat seat);
        int CreateSeat(Seat[] seat);
        int UpdateSeat(Seat seat);
        int DeleteSeat(long id);
    }
}
