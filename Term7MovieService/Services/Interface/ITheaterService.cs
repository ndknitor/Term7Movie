
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Response.Theater;

namespace Term7MovieService.Services.Interface
{
    public interface ITheaterService
    {
        Task<TheaterListResponse> GetTheatersAsync(TheaterFilterRequest request, long userId, string role);
        Task<TheaterResponse> GetTheaterByIdAsync(int id);
        Task<ParentResponse> CreateTheaterAsync(TheaterCreateRequest request, long managerId);
        Task<ParentResponse> UpdateTheaterAsync(TheaterUpdateRequest request);
        Task<ParentResponse> DeleteTheaterAsync(int id);
        Task<Location> GetLocationByAddressAsync(string address);
        Task<TheaterNameResponse> GetTheaterNamesFromCompany(int companyId, long? managerid);
        Task<ParentResponse> UpdateTheaterDefaultPrice(long managerId, TheaterDefaultPriceUpdateRequest request);
    }
}
