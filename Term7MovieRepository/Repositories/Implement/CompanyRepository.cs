using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieCore.Entities;
using Term7MovieCore.Data.Options;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Dto;
using Dapper;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Collections;
using Microsoft.EntityFrameworkCore;

namespace Term7MovieRepository.Repositories.Implement
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;

        private const string FILTER_WITH_PAGING = "Paging";
        private const string FILTER_WITH_NO_MANAGER = "NoManager";
        public CompanyRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<PagingList<CompanyDto>> GetAllCompany(CompanyFilterRequest request)
        {
            PagingList<CompanyDto> pagingList = new PagingList<CompanyDto>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize*(request.Page - 1);
                int fetch = request.PageSize;

                string sql = @" SELECT Id, Name, LogoUrl, IsActive 
                                FROM Companies 
                                ORDER BY Id 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(Id) 
                                FROM Companies ; ";

                string theaterQuery = @" SELECT Id, Name, Address, CompanyId, ManagerId, Status, Latitude, Longitude
                                         FROM Theaters ";

                object param = new { fetch, offset };

                var multiQ = await con.QueryMultipleAsync(sql + count + theaterQuery, param);

                IEnumerable<CompanyDto> list = await multiQ.ReadAsync<CompanyDto>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                IEnumerable<TheaterDto> theaters = await multiQ.ReadAsync<TheaterDto>();

                foreach(var company in list)
                {
                    company.Theaters = theaters.Where(t => t.CompanyId == company.Id);
                }

                pagingList = new PagingList<CompanyDto>(page: request.Page, pageSize: request.PageSize, results: list, total: total);
            }

            return pagingList;
        }
        public async Task<CompanyDto> GetCompanyById(int id)
        {
            CompanyDto company = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT Id, Name, LogoUrl, IsActive 
                                FROM Companies 
                                WHERE Id = @id ; ";

                string theaterQuery = @" SELECT Id, Name, Address, CompanyId, ManagerId, Status, Latitude, Longitude
                                         FROM Theaters
                                         WHERE CompanyId = @id ";
                object param = new { id };

                var multiQ = await con.QueryMultipleAsync(sql + theaterQuery, param);

                company = await multiQ.ReadFirstOrDefaultAsync<CompanyDto>();

                if (company != null)
                {
                    company.Theaters = await multiQ.ReadAsync<TheaterDto>();
                }
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
                                WHERE ManagerId = @managerId ; ";

                string theaterQuery = @" SELECT Id, Name, Address, CompanyId, ManagerId, Status, Latitude, Longitude
                                         FROM Theaters
                                         WHERE ManagerId = @managerId ";

                object param = new { managerId };

                var multiQ = await con.QueryMultipleAsync(sql + theaterQuery, param);

                company = await multiQ.ReadFirstOrDefaultAsync<CompanyDto>();

                if (company != null)
                {
                    company.Theaters = await multiQ.ReadAsync<TheaterDto>();
                }
            }
            return company;
        }

        public async Task<IEnumerable<CompanyDto>> GetAllCompanyNoPaging()
        {
            IEnumerable<CompanyDto> company = null;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = @" SELECT Id, Name, LogoUrl, IsActive, ManagerId  
                                FROM Companies 
                                WHERE IsActive = 1 ";

                company = await con.QueryAsync<CompanyDto>(sql);
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

        public async Task<long?> GetManagerIdFromCompanyId(int companyid)
        {
            if (!await _context.Database.CanConnectAsync())
                throw new Exception("DBCONNECTION");
            return _context.Companies.FirstOrDefault(a => a.Id == companyid).ManagerId;
            //long? company = null;
            //using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            //{
            //    string sql = @" SELECT TOP(1) ManagerId
            //                    FROM Companies 
            //                    WHERE Id = @companyid ";
            //    object param = new { companyid };
            //    company = await con.QueryFirstOrDefaultAsync<long>(sql, param);
            //}
            //return company;
        }

        private string GetAdditionFilterQuery(CompanyFilterRequest request, string filter)
        {
            string query = "";
            return query;
        }
    }
}
