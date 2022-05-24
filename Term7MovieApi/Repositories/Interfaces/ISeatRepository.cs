using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ISeatRepository
    {
        IEnumerable<Seat> GetAllSeat(int roomId);
        Seat GetSeatById(long id);
        long CreateSeat(Seat seat);
        long CreateSeat(Seat[] seat);
        long UpdateSeat(Seat seat);
        long DeleteSeat(long id);
    }
}
