using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITicketStatusRepository
    {
        IEnumerable<TicketStatus> GetAllTicketStatus();
    }
}
