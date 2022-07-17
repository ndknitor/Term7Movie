using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITransactionHistoryRepository
    {
        Task<PagingList<TransactionHistory>> GetAllTransactionHistory(ParentFilterRequest request);
        Task<PagingList<TransactionHistory>> GetAllTransactionHistoryByCustomerId(ParentFilterRequest request, long customerId);
        Task<PagingList<TransactionHistory>> GetAllTransactionHistoryByCompanyId(ParentFilterRequest request, long managerId);
        TransactionHistory GetTransactionHistoryById(long id);
        Task CreateTransactionHistory(IEnumerable<long> idList);
        Task<TicketSoldDTO> GetTicketSoldInTwoRecentWeek(long managerid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentWeek(long managerid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<TicketSoldDTO> GetTicketSoldInTwoRecentWeek(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentWeek(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<TicketSoldDTO> GetTicketSoldInTwoRecentMonth(long managerid, /*bool Comparable,*/
            DateTime ThisFirstMonth, DateTime FirstPreviousMonth, DateTime LastPreviousMonth);
        Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentMonth(long managerid, /*bool Comparable,*/
            DateTime ThisFirstMonth, DateTime FirstPreviousMonth, DateTime LastPreviousMonth);
        Task<TicketSoldDTO> GetTicketSoldInTwoRecentMonth(DateTime ThisFirstMonth, 
            DateTime FirstPreviousMonth, DateTime LastPreviousMonth);
        Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentMonth(DateTime ThisFirstMonth,
            DateTime FirstPreviousMonth, DateTime LastPreviousMonth);
        Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year, long managerid);
        Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year);
    }
}
