using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class ShowtimeTicketTypeRepository : IShowtimeTicketTypeRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        public ShowtimeTicketTypeRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<IEnumerable<ShowtimeTicketTypeDto>> GetShowtimeTicketTypeByShowtimeId(long showtimeId)
        {
            IEnumerable<ShowtimeTicketTypeDto> list = null;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT shtt.Id, shtt.ShowtimeId, shtt.TicketTypeId, shtt.ReceivePrice, 
                              sh.Id, sh.MovieId, sh.RoomId, sh.StartTime, sh.EndTime, sh.TheaterId,
                              tt.Id, tt.Name, tt.CompanyId  
                       FROM ShowtimeTicketTypes shtt JOIN Showtimes sh ON shtt.ShowtimeId = sh.Id
                            JOIN TicketTypes tt ON shtt.TicketTypeId = tt.Id 
                       WHERE shtt.ShowtimeId = @showtimeId AND sh.StartTime > GETUTCDATE() ";

                object param = new { showtimeId };

                list = await con.QueryAsync<ShowtimeTicketTypeDto, ShowtimeDto, TicketTypeDto, ShowtimeTicketTypeDto>(sql, (shtt, sh, tt) =>
                {
                    shtt.Showtime = sh;
                    shtt.TicketType = tt;
                    return shtt;
                }, param, splitOn: "Id");
            }
            return list;
        }

        public async Task<int> InsertShowtimeTicketType(ShowtimeTicketTypeCreateRequest request)
        {
            int count = 0;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" INSERT INTO ShowtimeTicketTypes (Id, ShowtimeId, TicketTypeId, ReceivePrice) 
                       SELECT NEWID(), @ShowtimeId, @TicketTypeId, @ReceivePrice
                       FROM Showtimes sh
                       WHERE StartTime > GETUTCDATE() 
                             AND sh.Id = @ShowtimeId
                    ";

                count = await con.ExecuteAsync(sql, request);
            }

            return count;
        }
    }
}
