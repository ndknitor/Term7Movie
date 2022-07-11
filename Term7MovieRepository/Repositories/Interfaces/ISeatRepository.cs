using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<SeatDto>> GetRoomSeats(int roomId);
        Task<SeatDto> GetSeatById(long id);
        Task CreateSeat(Seat seat);
        Task CreateSeat(IEnumerable<Seat> seats);
        Task UpdateSeat(Seat seat);
        Task DeleteSeat(long id);
    }
}
