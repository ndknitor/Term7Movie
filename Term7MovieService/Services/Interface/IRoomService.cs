using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Response.Room;

namespace Term7MovieService.Services.Interface
{
    public interface IRoomService
    {
        Task<RoomResponse> GetRoomDetail(int roomId);
        Task<ParentResultResponse> GetRoomsByTheaterId(RoomFilterRequest request);
        Task<ParentResponse> CreateRoom(RoomCreateRequest request);
        Task<ParentResponse> UpdateRoom(RoomUpdateRequest request);
        Task<ParentResponse> DeleteRoom(int id);
        Task<RoomNumberResponse> GetRoomNumberFromTheater(int theaterid);
    }
}
