﻿using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<IEnumerable<Ticket>> GetAllTicketByShowtime(int showtimeid);
        Task<IEnumerable<Ticket>> GetAllTicketByTransactionId(Guid id);
        Task<Ticket> GetTicketById(long id);
        Task BuyTicket(long transactionId); //chưa cần
        Task CreateTicket(Ticket ticket);
        Task CreateTicket(IEnumerable<Ticket> tickets);
        Task DeleteExpiredTicket();
        Task<IEnumerable<Ticket>> GetTicketByIdListAsync(IEnumerable<long> idList);
        Task<bool> IsTicketInShowtimeValid(long showtimeId, IEnumerable<long> ticketId);
        Task LockTicketAsync(IEnumerable<long> idList);
    }
}
