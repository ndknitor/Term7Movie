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

        public async Task<IEnumerable<SeatDto>> GetRoomSeats(int roomId)
        {
            IEnumerable<SeatDto> list = new List<SeatDto>();

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT s.Id, s.Name, RoomId, ColumnPos, RowPos, SeatTypeId, st.Id, st.Name " +
                    " FROM Seats s JOIN SeatTypes st ON s.SeatTypeId = st.Id " +
                    " WHERE RoomId = @roomId AND s.Status = 1 ";

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

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " SELECT s.Id, s.Name, RoomId, ColumnPos, RowPos, SeatTypeId, st.Id, st.Name " +
                    " FROM Seats s JOIN SeatTypes st ON s.SeatTypeId = st.Id " +
                    " WHERE s.Id = @id AND s.Status = 1 ";
                object param = new { id };
                seat = await con.QueryFirstOrDefaultAsync<SeatDto>(sql, param);
            }

            return seat;
        }
        public async Task CreateSeat(Seat seat)
        {
            await _context.AddAsync(seat);
        }
        public async Task CreateSeat(IEnumerable<Seat> seats)
        {
            await _context.AddRangeAsync(seats);
        }
        public async Task UpdateSeat(Seat seat)
        {
            Seat dbSeat = await _context.Seats.FindAsync(seat.Id);

            if (dbSeat == null) return;

            dbSeat.ColumnPos = seat.ColumnPos;
            dbSeat.RowPos = seat.RowPos;
            dbSeat.Name = seat.Name;
            dbSeat.SeatTypeId = seat.SeatTypeId;
            dbSeat.Status = seat.Status;
        }
        public async Task DeleteSeat(long id)
        {
            Seat dbSeat = await _context.Seats.FindAsync(id);

            if (dbSeat == null) return;

            dbSeat.Status = false;
        }
    }
}
