using Term7MovieApi.Entities;

namespace Term7MovieApi.Repositories.Interfaces
{
    public interface ITicketStatusRepository
    {
        IEnumerable<TicketStatus> GetAllTicketStatus();
    }
}
