using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAllTicketByShowtime(Showtime showtime);
        IEnumerable<Ticket> GetAllTicketByTransactionId(long id);
        Ticket GetTicketById(long id);
        int BuyTicket(long transactionId);
        int CreateTicket(Ticket ticket);
        int CreateTicket(Ticket[] tickets);
        int DeleteExpiredTicket();
    }
}
