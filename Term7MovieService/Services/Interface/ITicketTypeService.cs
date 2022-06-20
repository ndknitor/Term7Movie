using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITicketTypeService
    {
        Task<ParentResultResponse> GetAllTicketTypeAsync(long managerId);
        Task<ParentResponse> CreateTicketType(TicketTypeCreateRequest request, long managerId);
        Task<ParentResponse> UpdateTicketType(TicketTypeUpdateRequest request);
        Task<ParentResultResponse> GetTicketTypeByIdAsync(long id);
    }
}
