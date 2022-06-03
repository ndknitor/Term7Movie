
using System.Collections.Generic;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ISeatService
    {
        Task<SeatResponse> GetSeatById(long id);
        Task<SeatListResponse> GetRoomSeats(int roomId);
        Task<ParentResponse> CreateSeat(IEnumerable<SeatCreateRequest> request);
        Task<ParentResponse> UpdateSeat(SeatUpdateRequest request);
        Task<ParentResponse> DeleteSeat(long id);
    }
}   
