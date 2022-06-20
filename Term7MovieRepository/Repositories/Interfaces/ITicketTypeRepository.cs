using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITicketTypeRepository
    {
        Task<IEnumerable<TicketTypeDto>> GetAllTicketTypeByManagerIdAsync(long managerId);
        Task<int> CreateAsync(TicketTypeCreateRequest request, long managerId);
        Task<int> UpdateAsync(TicketTypeUpdateRequest request);
    }
}
