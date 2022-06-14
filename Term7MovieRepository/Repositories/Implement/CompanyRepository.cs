using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Options;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Dto;
using Dapper;
using Term7MovieCore.Data.Request;

namespace Term7MovieRepository.Repositories.Implement
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public CompanyRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<IEnumerable<CompanyDto>> GetAllCompany(ParentFilterRequest request)
        {
            IEnumerable<CompanyDto> list = new List<CompanyDto>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize*(request.Page - 1);
                int fetch = request.PageSize;

                string sql = @" SELECT Id, Name, LogoUrl, IsActive 
                                FROM Companies 
                                ORDER BY Id 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ";
                object param = new { fetch, offset };
                list = await con.QueryAsync<CompanyDto>(sql, param);
            }

            return list;
        }
        public async Task<CompanyDto> GetCompanyById(int id)
        {
            CompanyDto company = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT Id, Name, LogoUrl, IsActive 
                                FROM Companies 
                                WHERE Id = @id ";
                object param = new { id };
                company = await con.QueryFirstOrDefaultAsync<CompanyDto>(sql, param);
            }
            return company;
        }

        public async Task<CompanyDto> GetCompanyByManagerId(long managerId)
        {
            CompanyDto company = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT Id, Name, LogoUrl, IsActive 
                                FROM Companies 
                                WHERE ManagerId = @managerId ";
                object param = new { managerId };
                company = await con.QueryFirstOrDefaultAsync<CompanyDto>(sql, param);
            }
            return company;
        }
        public int CreateCompany(TheaterCompany company)
        {
            int count = 0;
            return count;
        }
        public int UpdateCompany(TheaterCompany company)
        {
            int count = 0;
            return count;
        }
        public int DeleteCompany(int id)
        {
            int count = 0;
            return count;
        }
    }
}
