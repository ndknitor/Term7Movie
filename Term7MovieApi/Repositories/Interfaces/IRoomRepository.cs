using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
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
