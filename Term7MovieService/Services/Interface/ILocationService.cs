using Term7MovieCore.Data.Dto;

namespace Term7MovieService.Services.Interface
{
    public interface ILocationService
    {
        Task<Location> GetLocationByAddressAsync(string address);
    }
}
