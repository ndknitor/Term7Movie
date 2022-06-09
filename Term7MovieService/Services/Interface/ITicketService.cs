using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITicketService
    {
        Task<TicketResponse> GetTicketForAShowTime(int showtimeid);

        Task<TicketResponse> GetTicketForATransaction(Guid transactionid);

        Task<TicketResponse> GetDetailOfATicket(long id);
    }
}
