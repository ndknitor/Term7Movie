using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class TicketTypeRepository : ITicketTypeRepository
    {
        private readonly AppDbContext _context;

        private readonly ConnectionOption _connectionOption;

        public TicketTypeRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<IEnumerable<TicketTypeDto>> GetAllTicketTypeByManagerIdAsync(long managerId)
        {
            IEnumerable<TicketTypeDto> list = null;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT tt.Id, tt.Name, tt.CompanyId 
                       FROM TicketTypes tt JOIN Companies c ON  tt.CompanyId = c.Id
                       WHERE c.ManagerId = @managerId ";

                object param = new { managerId };

                list = await con.QueryAsync<TicketTypeDto>(sql, param);
            }

            return list;
        }

        public async Task<TicketTypeDto> GetTicketTypeByIdAsync(long id)
        {
            TicketTypeDto type = null;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT Id, Name, CompanyId
                       FROM TicketTypes 
                       WHERE Id = @id ";

                object param = new { id };

                type = await con.QueryFirstOrDefaultAsync<TicketTypeDto>(sql, param);
            }

            return type;
        }

        public async Task<int> CreateAsync(TicketTypeCreateRequest request, long managerId)
        {
            int count = 0;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" INSERT INTO TicketTypes (Name, CompanyId) 
                       SELECT @Name, Id
                       FROM Companies 
                       WHERE ManagerId = @managerId ";
                object param = new
                {
                    request.Name,
                    managerId
                };
                count = await con.ExecuteAsync(sql, param);
            }

            return count;
        }

        public async Task<int> UpdateAsync(TicketTypeUpdateRequest request)
        {
            TicketType ticket = await _context.TicketTypes.FindAsync(request.Id);

            if (ticket == null) throw new BadRequestException(ErrorMessageConstants.ERROR_MESSAGE_INVALID_TICKET_TYPE_ID);

            ticket.Name = request.Name;

            return await _context.SaveChangesAsync();
        }

        public async Task<bool> CanManagerAccessTicketType(long ticketTypeId, long managerId)
        {
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" SELECT 1
                       FROM TicketTypes tt JOIN Companies c ON tt.CompanyId = c.Id 
                       WHERE c.ManagerId = @managerId AND tt.Id = @ticketTypeId  ";

                object param = new
                {
                    ticketTypeId,
                    managerId
                };

                int count = await con.QueryFirstOrDefaultAsync<int>(sql, param);

                return count == 1;
            }
        }
    }
}
