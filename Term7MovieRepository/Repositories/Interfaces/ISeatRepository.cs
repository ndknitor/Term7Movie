using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        Task<IEnumerable<Seat>> GetAllSeat(int roomId);
        Task<Seat> GetSeatById(long id);
        int CreateSeat(Seat seat);
        int CreateSeat(Seat[] seat);
        int UpdateSeat(Seat seat);
        int DeleteSeat(long id);
    }
}
