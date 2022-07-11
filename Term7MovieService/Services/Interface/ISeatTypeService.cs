using System.Threading.Tasks;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ISeatTypeService
    {
        Task<SeatTypeListResponse> GetAllSeatTypeAsync();
        Task<SeatTypeResponse> GetSeatTypeByIdAsync(int id);
        Task<ParentResponse> UpdateSeatTypeAsync(SeatTypeUpdateRequest request);
    }
}
