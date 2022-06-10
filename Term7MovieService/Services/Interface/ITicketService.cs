using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITicketService
    {
        Task<TicketResponse> GetTicketForSomething(TicketRequest request);

        //Task<TicketResponse> GetTicketForAShowTime(int showtimeid);

        //Task<TicketResponse> GetTicketForATransaction(Guid transactionid);

        Task<TicketResponse> GetDetailOfATicket(long id);

        Task<ParentResponse> CreateTicket(TicketCreateRequest request);

        Task<ParentResponse> CreateALotOfTicket(TicketCreateRequest[] request);
    }
}
