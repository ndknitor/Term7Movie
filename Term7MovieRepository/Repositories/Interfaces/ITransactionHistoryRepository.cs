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
        Task<TicketSoldDTO> GetQuickTicketSoldInTwoRecentWeek(int companyid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IncomeDTO> GetQuickTicketStonkOrStinkInTwoRecentWeek(int companyid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<TicketSoldDTO> GetQuickTicketSoldInTwoRecentWeek(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IncomeDTO> GetQuickTicketStonkOrStinkInTwoRecentWeek(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year, int companyid);
        Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year);
    }
}
