﻿using Term7MovieCore.Data.Collections;
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
        Task<TicketSoldDTO> GetQuickTicketSoldInTwoRecentWeekAsync(long managerid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IncomeDTO> GetQuickTicketStonkOrStinkInTwoRecentWeekAsync(long managerid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<TicketSoldDTO> GetQuickTicketSoldInTwoRecentWeekAsync(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IncomeDTO> GetQuickTicketStonkOrStinkInTwoRecentWeekAsync(DateTime ThisMondayWeek, 
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year, long managerid);
        Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year);

        //TicketSoldDTO GetQuickTicketSoldInTwoRecentWeek(long managerid, /*bool Comparable,*/
        //    DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        //IncomeDTO GetQuickTicketStonkOrStinkInTwoRecentWeek(long managerid, /*bool Comparable,*/
        //    DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        //TicketSoldDTO GetQuickTicketSoldInTwoRecentWeek(DateTime ThisMondayWeek,
        //    DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
        //IncomeDTO GetQuickTicketStonkOrStinkInTwoRecentWeek(DateTime ThisMondayWeek,
        //    DateTime MondayPreviousWeek, DateTime SundayPreviousWeek);
    }
}
