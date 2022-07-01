using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
﻿using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITicketRepository
    {
        Task<PagingList<TicketDto>> GetAllTicketAsync(TicketFilterRequest request);
        Task<IEnumerable<Ticket>> GetAllTicketByShowtime(int showtimeid);
        Task<IEnumerable<Ticket>> GetAllTicketByTransactionId(Guid id);
        Task<TicketDto> GetTicketById(long id, bool isNotShowed);
        Task<bool> BuyTicket(Guid transactionId, IEnumerable<long> idList);
        Task<int> CreateTicketAsync(TicketListCreateRequest request);
        Task DeleteExpiredTicket();
        IEnumerable<TicketDto> GetTicketByIdList(IEnumerable<long> idList);
        Task<bool> IsTicketInShowtimeValid(long showtimeId, IEnumerable<long> ticketId);
        void LockTicket(IEnumerable<long> idList);
        Task<Tuple<decimal, decimal>> GetMinAndMaxPriceFromShowTimeId(long showtimeid);
        //Task<TicketQuanityDTO> GetQuickTicketQuanityInTwoWeek(int companyid, 
        //    DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
    }
}
