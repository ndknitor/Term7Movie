using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class SeatTypeRepository : ISeatTypeRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public SeatTypeRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<IEnumerable<SeatTypeDto>> GetAllSeatTypeAsync()
        {
            IEnumerable<SeatTypeDto> list = new List<SeatTypeDto>();

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT Id, Name, BonusPrice " +
                    " FROM SeatTypes ";
                list = await con.QueryAsync<SeatTypeDto>(sql);
            }

            return list;
        }
        public async Task<SeatTypeDto> GetSeatTypeByIdAsync(int id)
        {
            SeatTypeDto dto = null;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT Id, Name, BonusPrice " +
                    " FROM SeatTypes " +
                    " WHERE Id = @id ";
                object param = new { id };
                dto = await con.QueryFirstOrDefaultAsync<SeatTypeDto>(sql, param);
            }

            return dto;
        }
        public async Task UpdateSeatTypeAsync(SeatType seatType)
        {
            SeatType dbSeatType = await _context.SeatTypes.FindAsync(seatType.Id);

            if (dbSeatType == null) return;

            dbSeatType.BonusPrice = seatType.BonusPrice;
        }

    }
}
