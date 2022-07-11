using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITicketTypeRepository
    {
        Task<IEnumerable<TicketTypeDto>> GetAllTicketTypeByManagerIdAsync(long managerId);
        Task<TicketTypeDto> GetTicketTypeByIdAsync(long id);
        Task<int> CreateAsync(TicketTypeCreateRequest request, long managerId);
        Task<int> UpdateAsync(TicketTypeUpdateRequest request);
        Task<bool> CanManagerAccessTicketType(long ticketTypeId, long managerId);
        Task<bool> CanManagerAccessTicketTypes(IEnumerable<long> ticketTypeId, long managerId);
    }
}
