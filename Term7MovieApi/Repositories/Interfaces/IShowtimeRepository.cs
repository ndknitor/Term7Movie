using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface IShowtimeRepository
    {
        IEnumerable<Showtime> GetAllShowtime(int theaterId);
        Showtime GetShowtimeById(long id);
        int CreateShowtime(Showtime showtime);
        int UpdateShowtime(Showtime showtime);
        int DeleteShowtimeById(long id);
    }
}
