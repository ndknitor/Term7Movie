using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto.Analyst;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TransactionHistoryRepository : ITransactionHistoryRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public TransactionHistoryRepository(AppDbContext context, ConnectionOption connection)
        {
            _context = context;
            _connectionOption = connection;
        }

        public async Task<PagingList<TransactionHistory>> GetAllTransactionHistory(ParentFilterRequest request)
        {
            PagingList<TransactionHistory> pagingList = new PagingList<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1);
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(Id) FROM TransactionHistories ";

                object param = new { fetch, offset };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                var list = await multiQ.ReadAsync<TransactionHistory>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TransactionHistory>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }

        public async Task<PagingList<TransactionHistory>> GetAllTransactionHistoryByCustomerId(ParentFilterRequest request, long customerId)
        {
            PagingList<TransactionHistory> pagingList = new PagingList<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1 );
                string sql = @" SELECT Id, UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId
                                FROM TransactionHistories
                                WHERE UserId = @customerId
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(Id) 
                                FROM TransactionHistories
                                WHERE UserId = @customerId ";

                object param = new { customerId, fetch, offset};

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                var list = await multiQ.ReadAsync<TransactionHistory>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TransactionHistory>(request.PageSize, request.Page, list, total);

            }

            return pagingList;
        }
        public async Task<PagingList<TransactionHistory>> GetAllTransactionHistoryByCompanyId(ParentFilterRequest request, long managerId)
        {
            PagingList<TransactionHistory> pagingList = new PagingList<TransactionHistory>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize * (request.Page - 1);

                string sql = @" SELECT trh.Id, trh.UserId, trh.TicketId, trh.TicketPrice, trh.TheaterId, trh.TheaterName, trh.PurchasedDate, trh.MovieId, trh.TransactionId
                                FROM TransactionHistories trh JOIN Theaters th ON trh.TheaterId = th.Id
                                WHERE th.ManagerId = @managerId
                                ORDER BY Id DESC 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(trh.Id) 
                                FROM TransactionHistories trh JOIN Theaters th ON trh.TheaterId = th.Id
                                WHERE th.ManagerId = @managerId ";

                object param = new { fetch, offset, managerId };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                var list = await multiQ.ReadAsync<TransactionHistory>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TransactionHistory>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }
        public TransactionHistory GetTransactionHistoryById(long id)
        {
            TransactionHistory history = null;
            return history;
        }
        public async Task CreateTransactionHistory(IEnumerable<long> idList)
        {
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " INSERT INTO TransactionHistories (UserId, TicketId, TicketPrice, TheaterId, TheaterName, PurchasedDate, MovieId, TransactionId) " +
                    " SELECT tr.CustomerId, tk.Id, tk.SellingPrice, th.Id, th.Name, tr.PurchasedDate, sh.MovieId, tr.Id " +
                    " FROM Tickets tk JOIN Showtimes sh ON tk.ShowTimeId = sh.Id " +
                    "   JOIN Theaters th ON sh.TheaterId = th.Id " +
                    "   JOIN Transactions tr ON tk.TransactionId = tr.Id " +
                    " WHERE tk.Id IN @idList ";
                object param = new { idList };

                await con.ExecuteAsync(sql, param);
            }
        }

        public async Task<TicketSoldDTO> GetTicketSoldInTwoRecentWeek(long managerid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            int TotalTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                        Theaterid = th.TheaterId,
                                                        Money = tic.ReceivePrice
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.Theaterid,
                                                    th => th.Id, (pretable, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = pretable.PurchasedDate,
                                                        ManagerId = th.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid)
                                                    .Select(a => a.ManagerId).CountAsync();
            int OldTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                        Theaterid = th.TheaterId,
                                                        Money = tic.ReceivePrice
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.Theaterid,
                                                    th => th.Id, (pretable, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = pretable.PurchasedDate,
                                                        ManagerId = th.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid 
                                                        && xxx.PurchasedDate <= SundayPreviousWeek 
                                                        && xxx.PurchasedDate >= MondayPreviousWeek)
                                                    .Select(a => a.ManagerId).CountAsync();
            int NewTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                        Theaterid = th.TheaterId,
                                                        Money = tic.ReceivePrice
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.Theaterid,
                                                    th => th.Id, (pretable, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = pretable.PurchasedDate,
                                                        ManagerId = th.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid
                                                        && xxx.PurchasedDate <= DateTime.UtcNow
                                                        && xxx.PurchasedDate >= ThisMondayWeek)
                                                    .Select(a => a.ManagerId).CountAsync();
            TicketSoldDTO dto = new TicketSoldDTO();
            dto.TotalTicketSoldQuantity = TotalTicketSold;
            dto.OldTicketSoldQuantity = OldTicketSold;
            dto.NewTicketSoldQuantity = NewTicketSold;
            if (NewTicketSold > OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = (float)100D - OldTicketSold * (float)100D / NewTicketSold;
            }
            else if (NewTicketSold < OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = false;
                dto.PercentTicketSoldChange = (float)100D - NewTicketSold * (float)100D / OldTicketSold;
            }
            else if (NewTicketSold == OldTicketSold)
            {
                //be positive :D
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = 0;
            }
            return dto;
        }

        public async Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentWeek(long managerid, /*bool Comparable,*/
            DateTime ThisMondayWeek, DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            //throw new NotImplementedException();
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            //I can taste the tension like a cloud of smoke in the air
            decimal TotalIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid)
                                                    .Select(a => a.Money).SumAsync();
            decimal OldIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid 
                                                                && xxx.PurchasedDate >= MondayPreviousWeek
                                                                && xxx.PurchasedDate <= SundayPreviousWeek)
                                                    .Select(a => a.Money).SumAsync();
            decimal NewIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid
                                                                && xxx.PurchasedDate >= ThisMondayWeek
                                                                && xxx.PurchasedDate <= DateTime.UtcNow)
                                                    .Select(a => a.Money).SumAsync();
            IncomeDTO dto = new IncomeDTO();
            dto.TotalIncome = TotalIncome;
            dto.OldIncome = OldIncome;
            dto.NewIncome = NewIncome;
            if(NewIncome > OldIncome)
            {
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = (decimal)100D - OldIncome * (decimal)100D / NewIncome;
            }
            else if (NewIncome < OldIncome)
            {
                dto.IsIncomeUpOrDown = false;
                dto.PercentIncomeChange = (decimal)100D - NewIncome * (decimal)100D / OldIncome;
            }
            else if( NewIncome == OldIncome)
            {
                //be positive :D
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = 0;
            }
            return dto;
        }

        public async Task<TicketSoldDTO> GetTicketSoldInTwoRecentWeek(DateTime ThisMondayWeek,
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            int TotalTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                    })
                                                    .Select(a => a.PurchasedDate).CountAsync();
            int OldTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate <= SundayPreviousWeek
                                                                && xxx.PurchasedDate >= MondayPreviousWeek)
                                                    .Select(a => a.PurchasedDate).CountAsync();
            int NewTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate <= DateTime.UtcNow
                                                                && xxx.PurchasedDate >= ThisMondayWeek)
                                                    .Select(a => a.PurchasedDate).CountAsync();
            TicketSoldDTO dto = new TicketSoldDTO();
            dto.TotalTicketSoldQuantity = TotalTicketSold;
            dto.OldTicketSoldQuantity = OldTicketSold;
            dto.NewTicketSoldQuantity = NewTicketSold;
            if (NewTicketSold > OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = (float)100D - OldTicketSold * (float)100D / NewTicketSold;
            }
            else if (NewTicketSold < OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = false;
                dto.PercentTicketSoldChange = (float)100D - NewTicketSold * (float)100D / OldTicketSold;
            }
            else if (NewTicketSold == OldTicketSold)
            {
                //be positive :D
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = 0;
            }
            return dto;
        }

        public async Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentWeek(DateTime ThisMondayWeek,
            DateTime MondayPreviousWeek, DateTime SundayPreviousWeek)
        {
            //throw new NotImplementedException();
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            //I can taste the tension like a cloud of smoke in the air
            decimal TotalIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Select(a => a.Money).SumAsync();
            decimal OldIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate >= MondayPreviousWeek
                                                                && xxx.PurchasedDate <= SundayPreviousWeek)
                                                    .Select(a => a.Money).SumAsync();
            decimal NewIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate >= ThisMondayWeek
                                                                && xxx.PurchasedDate <= DateTime.UtcNow)
                                                    .Select(a => a.Money).SumAsync();
            IncomeDTO dto = new IncomeDTO();
            dto.TotalIncome = TotalIncome;
            dto.OldIncome = OldIncome;
            dto.NewIncome = NewIncome;
            if (NewIncome > OldIncome)
            {
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = (decimal)100D - OldIncome * (decimal)100D / NewIncome;
            }
            else if (NewIncome < OldIncome)
            {
                dto.IsIncomeUpOrDown = false;
                dto.PercentIncomeChange = (decimal)100D - NewIncome * (decimal)100D / OldIncome;
            }
            else if (NewIncome == OldIncome)
            {
                //be positive :D
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = 0;
            }
            return dto;
        }

        public async Task<TicketSoldDTO> GetTicketSoldInTwoRecentMonth(long managerid, /*bool Comparable,*/
            DateTime ThisFirstMonth, DateTime FirstPreviousMonth, DateTime LastPreviousMonth)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            int TotalTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                        Theaterid = th.TheaterId,
                                                        Money = tic.ReceivePrice
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.Theaterid,
                                                    th => th.Id, (pretable, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = pretable.PurchasedDate,
                                                        ManagerId = th.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid)
                                                    .Select(a => a.ManagerId).CountAsync();
            int OldTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                        Theaterid = th.TheaterId,
                                                        Money = tic.ReceivePrice
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.Theaterid,
                                                    th => th.Id, (pretable, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = pretable.PurchasedDate,
                                                        ManagerId = th.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid
                                                        && xxx.PurchasedDate <= LastPreviousMonth
                                                        && xxx.PurchasedDate >= FirstPreviousMonth)
                                                    .Select(a => a.ManagerId).CountAsync();
            int NewTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                        Theaterid = th.TheaterId,
                                                        Money = tic.ReceivePrice
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.Theaterid,
                                                    th => th.Id, (pretable, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = pretable.PurchasedDate,
                                                        ManagerId = th.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid
                                                        && xxx.PurchasedDate <= DateTime.UtcNow
                                                        && xxx.PurchasedDate >= ThisFirstMonth)
                                                    .Select(a => a.ManagerId).CountAsync();
            TicketSoldDTO dto = new TicketSoldDTO();
            dto.TotalTicketSoldQuantity = TotalTicketSold;
            dto.OldTicketSoldQuantity = OldTicketSold;
            dto.NewTicketSoldQuantity = NewTicketSold;
            if (NewTicketSold > OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = (float)100D - OldTicketSold * (float)100D / NewTicketSold;
            }
            else if (NewTicketSold < OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = false;
                dto.PercentTicketSoldChange = (float)100D - NewTicketSold * (float)100D / OldTicketSold;
            }
            else if (NewTicketSold == OldTicketSold)
            {
                //be positive :D
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = 0;
            }
            return dto;
        }

        public async Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentMonth(long managerid, /*bool Comparable,*/
            DateTime ThisFirstMonth, DateTime FirstPreviousMonth, DateTime LastPreviousMonth)
        {
            //throw new NotImplementedException();
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            //I can taste the tension like a cloud of smoke in the air
            decimal TotalIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid)
                                                    .Select(a => a.Money).SumAsync();
            decimal OldIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid
                                                                && xxx.PurchasedDate >= FirstPreviousMonth
                                                                && xxx.PurchasedDate <= LastPreviousMonth)
                                                    .Select(a => a.Money).SumAsync();
            decimal NewIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.ManagerId == managerid
                                                                && xxx.PurchasedDate >= ThisFirstMonth
                                                                && xxx.PurchasedDate <= DateTime.UtcNow)
                                                    .Select(a => a.Money).SumAsync();
            IncomeDTO dto = new IncomeDTO();
            dto.TotalIncome = TotalIncome;
            dto.OldIncome = OldIncome;
            dto.NewIncome = NewIncome;
            if (NewIncome > OldIncome)
            {
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = (decimal)100D - OldIncome * (decimal)100D / NewIncome;
            }
            else if (NewIncome < OldIncome)
            {
                dto.IsIncomeUpOrDown = false;
                dto.PercentIncomeChange = (decimal)100D - NewIncome * (decimal)100D / OldIncome;
            }
            else if (NewIncome == OldIncome)
            {
                //be positive :D
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = 0;
            }
            return dto;
        }

        public async Task<TicketSoldDTO> GetTicketSoldInTwoRecentMonth(DateTime ThisFirstMonth,
            DateTime FirstPreviousMonth, DateTime LastPreviousMonth)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            int TotalTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                    })
                                                    .Select(a => a.PurchasedDate).CountAsync();
            int OldTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate <= LastPreviousMonth
                                                                && xxx.PurchasedDate >= FirstPreviousMonth)
                                                    .Select(a => a.PurchasedDate).CountAsync();
            int NewTicketSold = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        PurchasedDate = th.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate <= DateTime.UtcNow
                                                                && xxx.PurchasedDate >= ThisFirstMonth)
                                                    .Select(a => a.PurchasedDate).CountAsync();
            TicketSoldDTO dto = new TicketSoldDTO();
            dto.TotalTicketSoldQuantity = TotalTicketSold;
            dto.OldTicketSoldQuantity = OldTicketSold;
            dto.NewTicketSoldQuantity = NewTicketSold;
            if (NewTicketSold > OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = (float)100D - OldTicketSold * (float)100D / NewTicketSold;
            }
            else if (NewTicketSold < OldTicketSold)
            {
                dto.IsTicketSoldUpOrDown = false;
                dto.PercentTicketSoldChange = (float)100D - NewTicketSold * (float)100D / OldTicketSold;
            }
            else if (NewTicketSold == OldTicketSold)
            {
                //be positive :D
                dto.IsTicketSoldUpOrDown = true;
                dto.PercentTicketSoldChange = 0;
            }
            return dto;
        }

        public async Task<IncomeDTO> GetTicketStonkOrStinkInTwoRecentMonth(DateTime ThisFirstMonth,
            DateTime FirstPreviousMonth, DateTime LastPreviousMonth)
        {
            //throw new NotImplementedException();
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            //I can taste the tension like a cloud of smoke in the air
            decimal TotalIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money
                                                    })
                                                    .Select(a => a.Money).SumAsync();
            decimal OldIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate >= FirstPreviousMonth
                                                                && xxx.PurchasedDate <= LastPreviousMonth)
                                                    .Select(a => a.Money).SumAsync();
            decimal NewIncome = await _context.Tickets
                                                    .Join(_context.TransactionHistories,
                                                    tic => tic.Id,
                                                    th => th.TicketId, (tic, th) =>
                                                    new
                                                    {
                                                        Money = tic.ReceivePrice,
                                                        PurchasedDate = th.PurchasedDate,
                                                        TheaterId = th.TheaterId
                                                    })
                                                    .Join(_context.Theaters,
                                                    pretable => pretable.TheaterId,
                                                    theater => theater.Id, (pretable, theater) =>
                                                    new
                                                    {
                                                        ManagerId = theater.ManagerId,
                                                        Money = pretable.Money,
                                                        PurchasedDate = pretable.PurchasedDate,
                                                    })
                                                    .Where(xxx => xxx.PurchasedDate >= ThisFirstMonth
                                                                && xxx.PurchasedDate <= DateTime.UtcNow)
                                                    .Select(a => a.Money).SumAsync();
            IncomeDTO dto = new IncomeDTO();
            dto.TotalIncome = TotalIncome;
            dto.OldIncome = OldIncome;
            dto.NewIncome = NewIncome;
            if (NewIncome > OldIncome)
            {
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = (decimal)100D - OldIncome * (decimal)100D / NewIncome;
            }
            else if (NewIncome < OldIncome)
            {
                dto.IsIncomeUpOrDown = false;
                dto.PercentIncomeChange = (decimal)100D - NewIncome * (decimal)100D / OldIncome;
            }
            else if (NewIncome == OldIncome)
            {
                //be positive :D
                dto.IsIncomeUpOrDown = true;
                dto.PercentIncomeChange = 0;
            }
            return dto;
        }


        public async Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year, long managerid)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            List<YearlyIncomeDTO> result = new List<YearlyIncomeDTO>();
            bool IsItOldSchool = true;
            if (DateTime.UtcNow.Year == year)
                IsItOldSchool = false;
            if (IsItOldSchool)
            {
                for (int i = 1; i < 13; i++)
                {
                    //joiny joiny yes papa, 3 join statement? no papa, telling lies?, no papa, open your source code, huhuhu
                    var DateInMonth = GetFirstDateAndLastDateOfTheMonth(i, year);
                    var query = _context.TransactionHistories
                                                    .Join(_context.Tickets,
                                                        th => th.TicketId,
                                                        tic => tic.Id, (th, tic)
                                                         => new
                                                         {
                                                             PurchasedDate = th.PurchasedDate,
                                                             Income = tic.ReceivePrice,
                                                             Showtimeid = tic.ShowTimeId
                                                         })
                                                    .Join(_context.Showtimes,
                                                        pretable => pretable.Showtimeid,
                                                        st => st.Id, (pretable, st)
                                                        => new
                                                        {
                                                            PurchasedDate = pretable.PurchasedDate,
                                                            Income = pretable.Income,
                                                            Theaterid = st.TheaterId
                                                        })
                                                    .Join(_context.Theaters,
                                                        prepretable => prepretable.Theaterid,
                                                        theater => theater.Id, (prepretable, theater)
                                                        => new
                                                        {
                                                            PurchasedDate = prepretable.PurchasedDate,
                                                            Income = prepretable.Income,
                                                            Managerid = theater.ManagerId
                                                        })
                                                    .Where(xxx => xxx.PurchasedDate >= DateInMonth.Item1
                                                                    && xxx.PurchasedDate <= DateInMonth.Item2
                                                                    && xxx.Managerid == managerid)
                                                    .Select(xx => new YearlyIncomeDTO
                                                    {
                                                        Month = i,
                                                        Income = xx.Income
                                                    });
                    if (query.Any())
                    {
                        YearlyIncomeDTO dto = new YearlyIncomeDTO
                        {
                            Income = await query.SumAsync(x => x.Income),
                            Month = i
                        };
                        result.Add(dto);
                    }
                    else
                    {
                        YearlyIncomeDTO dto = new YearlyIncomeDTO
                        {
                            Income = 0,
                            Month = i
                        };
                        result.Add(dto);
                    }
                }
                return result;
            }
            for (int i = 1; i <= DateTime.UtcNow.Month; i++)
            {
                var DateInMonth = GetFirstDateAndLastDateOfTheMonth(i, year);
                var query = _context.TransactionHistories
                                                    .Join(_context.Tickets,
                                                        th => th.TicketId,
                                                        tic => tic.Id, (th, tic)
                                                         => new
                                                         {
                                                             PurchasedDate = th.PurchasedDate,
                                                             Income = tic.ReceivePrice,
                                                             Showtimeid = tic.ShowTimeId
                                                         })
                                                    .Join(_context.Showtimes,
                                                        pretable => pretable.Showtimeid,
                                                        st => st.Id, (pretable, st)
                                                        => new
                                                        {
                                                            PurchasedDate = pretable.PurchasedDate,
                                                            Income = pretable.Income,
                                                            Theaterid = st.TheaterId
                                                        })
                                                    .Join(_context.Theaters,
                                                        prepretable => prepretable.Theaterid,
                                                        theater => theater.Id, (prepretable, theater)
                                                        => new
                                                        {
                                                            PurchasedDate = prepretable.PurchasedDate,
                                                            Income = prepretable.Income,
                                                            Managerid = theater.ManagerId
                                                        })
                                                    .Where(xxx => xxx.PurchasedDate >= DateInMonth.Item1
                                                                    && xxx.PurchasedDate <= DateInMonth.Item2
                                                                    && xxx.Managerid == managerid)
                                                    .Select(xx => new YearlyIncomeDTO
                                                    {
                                                        Month = i,
                                                        Income = xx.Income
                                                    });
                if (query.Any())
                {
                    YearlyIncomeDTO dto = new YearlyIncomeDTO
                    {
                        Income = await query.SumAsync(x => x.Income),
                        Month = i
                    };
                    result.Add(dto);
                }
                else
                {
                    YearlyIncomeDTO dto = new YearlyIncomeDTO
                    {
                        Income = 0,
                        Month = i
                    };
                    result.Add(dto);
                }
            }
            return result;
        }

        public async Task<IEnumerable<YearlyIncomeDTO>> GetIncomeForAYear(int year)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new DbOperationException("DBCONNECTION");
            List<YearlyIncomeDTO> result = new List<YearlyIncomeDTO>();
            bool IsItOldSchool = true;
            if (DateTime.UtcNow.Year == year)
                IsItOldSchool = false;
            if(IsItOldSchool)
            {
                for (int i = 1; i < 13; i++)
                {
                    var DateInMonth = GetFirstDateAndLastDateOfTheMonth(i, year);
                    var query = _context.TransactionHistories
                                                    .Join(_context.Tickets,
                                                    th => th.TicketId,
                                                    tic => tic.Id, (th, tic)
                                                     => new
                                                     {
                                                         PurchasedDate = th.PurchasedDate,
                                                         Income = tic.ReceivePrice
                                                     })
                                                    .Where(xxx => xxx.PurchasedDate >= DateInMonth.Item1
                                                                    && xxx.PurchasedDate <= DateInMonth.Item2)
                                                    .Select(xx => new YearlyIncomeDTO
                                                    {
                                                        Month = i,
                                                        Income = xx.Income
                                                    });
                    if (query.Any())
                    {
                        YearlyIncomeDTO dto = new YearlyIncomeDTO
                        {
                            Income = await query.SumAsync(x => x.Income),
                            Month = i
                        };
                        result.Add(dto);
                    }
                    else
                    {
                        YearlyIncomeDTO dto = new YearlyIncomeDTO
                        {
                            Income = 0,
                            Month = i
                        };
                        result.Add(dto);
                    }
                }
                return result;
            }
            for(int i = 1; i <= DateTime.UtcNow.Month; i++)
            {
                var DateInMonth = GetFirstDateAndLastDateOfTheMonth(i, year);
                var query = _context.TransactionHistories
                                                .Join(_context.Tickets,
                                                th => th.TicketId,
                                                tic => tic.Id, (th, tic)
                                                 => new
                                                 {
                                                     PurchasedDate = th.PurchasedDate,
                                                     Income = tic.ReceivePrice
                                                 })
                                                .Where(xxx => xxx.PurchasedDate >= DateInMonth.Item1
                                                                && xxx.PurchasedDate <= DateInMonth.Item2)
                                                .Select(xx => new YearlyIncomeDTO
                                                {
                                                    Month = i,
                                                    Income = xx.Income
                                                });
                if (query.Any())
                {
                    YearlyIncomeDTO dto = new YearlyIncomeDTO
                    {
                        Income = await query.SumAsync(x => x.Income),
                        Month = i
                    };
                    result.Add(dto);
                }
                else
                {
                    YearlyIncomeDTO dto = new YearlyIncomeDTO
                    {
                        Income = 0,
                        Month = i
                    };
                    result.Add(dto);
                }
            }
            return result;
        }





        /* ----------------------- PRIVATE ZONE ----------------------- */
        private Tuple<DateTime, DateTime> GetFirstDateAndLastDateOfTheMonth(int month, int year)
        {
            DateTime FirstDate = new DateTime(year, month, 1);
            DateTime LastDate = FirstDate.AddMonths(1).AddDays(-1);
            return Tuple.Create(FirstDate, LastDate);

        }
        /* -------------------- END PRIVATE ZONE ---------------------- */




        //Backup zone for the what if the database change back to the old one lol
        //var TotalTicket = await _context.Tickets
        //                            .Where(a => TotalShowtime.Contains(a.ShowTimeId))
        //                            .Select(a => a.Id)
        //                            .ToListAsync();
        //var TotalIncome = await _context.TransactionHistories
        //                            .Where(a => TotalTicket.Contains(a.TicketId))
        //                            .Select(a => a.TicketId)
        //                            .ToListAsync();
        //Getting ticket id in 2 different timeline
        //var OldIncome = await _context.Showtimes
        //                        .Include(a => a.Tickets)
        //                        .Where(a => a.Tickets.Any
        //                        (a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1))
        //                            && a.StartTime <= SundayPreviousWeek && a.StartTime >= MondayPreviousWeek)
        //                        .SelectMany(a => a.Tickets)
        //                        .Where(a => _context.TransactionHistories.FirstOrDefault(xxx => xxx.TicketId == a.Id) == null ? false : true)
        //                        .SumAsync(a => a.ReceivePrice);
        //var NewIcome = await _context.Showtimes
        //                        .Include(a => a.Tickets)
        //                        .Where(a => a.Tickets.Any
        //                        (a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1))
        //                            && a.StartTime <= DateTime.UtcNow.Date && a.StartTime >= ThisMondayWeek)
        //                        .SelectMany(a => a.Tickets)
        //                        .Where(a => _context.TransactionHistories.FirstOrDefault(xxx => xxx.TicketId == a.Id) == null ? false : true)
        //                        .SumAsync(a => a.ReceivePrice);
        //var TotalIncome = await _context.Showtimes
        //                        .Include(a => a.Tickets)
        //                        .Where(a => a.Tickets.Any
        //                        (a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1)))
        //                        .SelectMany(a => a.Tickets)
        //                        .Where(a => _context.TransactionHistories.FirstOrDefault(xxx => xxx.TicketId == a.Id) == null ? false : true)
        //                        .SumAsync(a => a.ReceivePrice);
        //if (TotalIncome == 0 && NewIcome == 0 && OldIncome == 0)
        //{
        //    TotalIncome = 1;
        //    NewIcome = 1;
        //    OldIncome = 1;
        //}
        //IncomeDTO dto = new IncomeDTO();
        //dto.TotalIncome = TotalIncome;
        //dto.NewIncome = NewIcome;
        //dto.OldIncome = OldIncome;
        //if (NewIcome > OldIncome)
        //{
        //    dto.IsIncomeUpOrDown = true;
        //    dto.PercentIncomeChange = 100M - (OldIncome * 100M / NewIcome);
        //}
        //else if (NewIcome < OldIncome)
        //{
        //    dto.IsIncomeUpOrDown = false;
        //    dto.PercentIncomeChange = 100M - (NewIcome * 100M / OldIncome);
        //}
        //else if (NewIcome == OldIncome)
        //{
        //    dto.IsIncomeUpOrDown = true;
        //    dto.PercentIncomeChange = 0.69M;
        //}
        //return dto;


        ////Comprable mean it can be compare two week together to create 2 different number
        ////if (Comparable)
        ////{

        ////}

        //var TicketIds = await _context.Theaters
        //                    .Include(a => a.Showtimes)
        //                    .Where(a => a.CompanyId == 1)
        //                    .SelectMany(a => a.Showtimes)
        //                    .SelectMany(a => a.Tickets)
        //                    .Select(a => a.Id)
        //                    .ToListAsync();
        ////if (TicketIds.Count == 0) return null;
        //var showtimeids = await _context.Theaters
        //                    .Include(a => a.Showtimes)
        //                    .Where(a => a.CompanyId == 1)
        //                    .SelectMany(a => a.Showtimes)
        //                    .Select(a => a.Id).ToListAsync();
        ////Getting ticket id in 2 different timeline
        //var TicketIdsOld = await _context.Showtimes
        //                        .Include(a => a.Tickets)
        //                        .Where(a => a.Tickets.Any
        //                        (a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1))
        //                            && a.StartTime <= DateTime.UtcNow.Date && a.StartTime >= ThisMondayWeek)
        //                        .SelectMany(a => a.Tickets).Select(a => a.Id).ToListAsync();
        //var TicketIdsNew = await _context.Showtimes
        //                        .Include(a => a.Tickets)
        //                        .Where(a => a.Tickets.Any
        //                        (a => showtimeids.Contains(a.ShowTimeId != null ? a.ShowTimeId.Value : -1))
        //                            && a.StartTime <= SundayPreviousWeek && a.StartTime >= MondayPreviousWeek)
        //                        .SelectMany(a => a.Tickets).Select(a => a.Id).ToListAsync();
        ////start to track down the quanity sold successfully
        //var OldTicketSold = await _context.TransactionHistories
        //                                    .Where(a => TicketIdsOld.Contains(a.TicketId))
        //                                    .CountAsync();
        //var NewTicketSold = await _context.TransactionHistories
        //                                    .Where(a => TicketIdsNew.Contains(a.TicketId))
        //                                    .CountAsync();
        //var TotalTicketSold = await _context.TransactionHistories
        //                                    .CountAsync(a => TicketIds.Contains(a.TicketId));//oh so where wasn't necessary D:
        //if (OldTicketSold == 0 && NewTicketSold == 0 && TotalTicketSold == 0)
        //{
        //    NewTicketSold = 1;
        //    OldTicketSold = 1;
        //    TotalTicketSold = 1;
        //}
        //TicketSoldDTO dto = new TicketSoldDTO();
        //dto.TotalTicketSoldQuantity = TotalTicketSold;
        //dto.OldTicketSoldQuantity = OldTicketSold;
        //dto.NewTicketSoldQuantity = NewTicketSold;
        //if(NewTicketSold > OldTicketSold)
        //{
        //    dto.IsTicketSoldUpOrDown = true;
        //    dto.PercentTicketSoldChange = (float)100F - ((float)OldTicketSold * (float)100F / (float)NewTicketSold);
        //}
        //else if(NewTicketSold < OldTicketSold)
        //{
        //    dto.IsTicketSoldUpOrDown = false;
        //    dto.PercentTicketSoldChange = (float)100F - ((float)NewTicketSold * (float)100F / (float)OldTicketSold);
        //}
        //else if(NewTicketSold == OldTicketSold)
        //{
        //    dto.IsTicketSoldUpOrDown = true;
        //    dto.PercentTicketSoldChange = 0.69F;
        //}
        //return dto;
    }
}
