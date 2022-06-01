using Term7MovieCore.Data;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;


namespace Term7MovieService.Services.Interface
{
    public interface IRoomService
    {
        Task<RoomResponse> GetRoomDetail(int roomId);
        Task<TheaterRoomsResponse> GetRoomsByTheaterId(int theaterId);
        Task<ParentResponse> CreateRoom(RoomCreateRequest request);
    }
}
