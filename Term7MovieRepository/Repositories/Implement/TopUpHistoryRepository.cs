using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TopUpHistoryRepository : ITopUpHistoryRepository
    {
        private readonly AppDbContext context;
        private readonly ConnectionOption connectionOption;


        private const string FILTER_BY_EMAIL = "Email";
        private const string FILTER_BY_USER = "User";

        public TopUpHistoryRepository (AppDbContext context, ConnectionOption connectionOption)
        {
            this.context = context;
            this.connectionOption = connectionOption;
        }

        public async Task<PagingList<TopUpHistoryDto>> GetAllAsync(TopUpHistoryFilterRequest request)
        {
            PagingList<TopUpHistoryDto> pagingList = null;

            using (var con = new SqlConnection(connectionOption.FCinemaConnection))
            {
                int offset = (request.Page - 1) * request.PageSize;
                int fetch = request.PageSize;

                string sql =
                    @" SELECT tuh.Id, tuh.UserId, tuh.RecordDate, tuh.Amount, tuh.Description, tuh.TransactionId " +
                    (request.IncludeUser ? " , u.Id, u.Email, u.FullName, u.StatusId, us.Name 'StatusName' " : "") +
                    @" FROM TopUpHistories tuh 
                            JOIN Users u ON tuh.UserId = u.Id 
                            JOIN UserStatus us ON u.StatusId = us.Id 
                       WHERE 1=1 " +
                    GetAdditionalFilter(request, FILTER_BY_USER) +
                    GetAdditionalFilter(request, FILTER_BY_USER) +
                    @" ORDER BY tuh.Id DESC
                       OFFSET @offset ROWS 
                       FETCH NEXT @fetch ROWS ONLY ; ";

                string count =
                    @" SELECT COUNT(*) 
                       FROM TopUpHistories tuh 
                            JOIN Users u ON tuh.UserId = u.Id 
                            JOIN UserStatus us ON u.StatusId = us.Id 
                       WHERE 1=1 " +
                    GetAdditionalFilter(request, FILTER_BY_USER) +
                    GetAdditionalFilter(request, FILTER_BY_USER);


                object param = new { offset, fetch, request.UserId, request.Email};

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                IEnumerable<TopUpHistoryDto> list;

                if (request.IncludeUser)
                {
                    list = multiQ.Read<TopUpHistoryDto, UserDTO, TopUpHistoryDto>((tuh, u) =>
                    {
                        tuh.User = u;
                        return tuh;
                    });
                } else
                {
                    list = await multiQ.ReadAsync<TopUpHistoryDto>();
                }

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                pagingList = new PagingList<TopUpHistoryDto>(request.PageSize, request.Page, list, total);
            }

            return pagingList;
        }

        public async Task CreateTopUpHistory(TopUpHistory topUpHistory)
        {
            User user = await context.Users.FindAsync(topUpHistory.UserId);

            if (user == null) throw new BadRequestException($"User's Id {topUpHistory.UserId} not found");

            if (topUpHistory.Amount <= 0 && user.Point < -topUpHistory.Amount)
            {
                throw new BadRequestException($"User's wallet amount is insufficient");
            }

            user.Point += topUpHistory.Amount;
            await context.TopUpHistories.AddAsync(topUpHistory);
        }

        private string GetAdditionalFilter(TopUpHistoryFilterRequest request, string filter)
        {
            string sql = "";

            switch(filter)
            {
                case FILTER_BY_USER:
                    if (request.UserId != null)
                    {
                        sql = " AND tuh.UserId = @UserId ";
                    }
                    break;
                case FILTER_BY_EMAIL:
                    if (!string.IsNullOrEmpty(request.Email))
                    {
                        sql = " AND u.Email = @Email ";
                    }
                    break;
            }

            return sql;
        }
    }
}
