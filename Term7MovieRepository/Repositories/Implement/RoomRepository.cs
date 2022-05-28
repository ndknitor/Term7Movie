using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class RoomRepository : IRoomRepository
    {
        private AppDbContext _context;
        public RoomRepository(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Room> GetAllRoom(int theaterId)
        {
            IEnumerable<Room> list = new List<Room>();
            return list;
        }
        public Room GetRoomById(int id)
        {
            Room room = null;
            return room;
        }
        public int CreateRoom(Room room)
        {
            int count = 0;
            return count;
        }
        public int UpdateRoom(Room room)
        {
            int count = 0;
            return count;
        }
        public int DeleteRoom(int id)
        {
            int count = 0;
            return count;
        }
    }
}
