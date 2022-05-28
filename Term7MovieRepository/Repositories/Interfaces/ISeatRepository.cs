using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        IEnumerable<Seat> GetAllSeat(int roomId);
        Seat GetSeatById(long id);
        int CreateSeat(Seat seat);
        int CreateSeat(Seat[] seat);
        int UpdateSeat(Seat seat);
        int DeleteSeat(long id);
    }
}
