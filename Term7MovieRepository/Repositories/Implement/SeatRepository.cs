using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class SeatRepository : ISeatRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public SeatRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<IEnumerable<SeatDto>> GetAllSeat(int roomId)
        {
            IEnumerable<SeatDto> list = new List<SeatDto>();

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT s.Id, s.Name, RoomId, ColumnPos, RowPos, SeatTypeId, st.Id, st.Name, st.BonusPrice " +
                    " FROM Seats s JOIN SeatTypes st ON s.SeatTypeId = st.Id " +
                    " WHERE RoomId = @roomId ";

                var param = new { roomId };
                list = (await con.QueryAsync<SeatDto, SeatTypeDto, SeatDto>(sql, (s, st) =>
                {
                    s.SeatType = st;
                    return s;
                }, param: param, splitOn: "Id")).ToList();
            }

            return list;
        }
        public async Task<SeatDto> GetSeatById(long id)
        {
            SeatDto seat = null;
            return seat;
        }
        public int CreateSeat(Seat seat)
        {
            int count = 0;
            return count;
        }
        public int CreateSeat(Seat[] seat)
        {
            int count = 0;
            return count;
        }
        public int UpdateSeat(Seat seat)
        {
            int count = 0;
            return count;
        }
        public int DeleteSeat(long id)
        {
            int count = 0;
            return count;
        }
    }
}
