using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        IEnumerable<Room> GetAllRoom(int theaterId);
        Room GetRoomById(int id);
        int CreateRoom(Room room);
        int UpdateRoom(Room room);
        int DeleteRoom(int id);
    }
}
