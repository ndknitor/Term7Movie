using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITicketService
    {
        Task<ParentResultResponse> GetTicketAsync(TicketFilterRequest request);
        Task<TicketResponse> GetTicketForSomething(TicketRequest request);

        //Task<TicketResponse> GetTicketForAShowTime(int showtimeid);

        //Task<TicketResponse> GetTicketForATransaction(Guid transactionid);

        Task<ParentResultResponse> GetTicketDetail(long id, long showtimeId, string role);

        Task<ParentResponse> CreateTicketAsync(TicketListCreateRequest request);

        Task<ParentResponse> LockTicketAsync(LockTicketRequest request);

        Task<ParentResultResponse> GetTicketOnSellingAsync();
    }
}
