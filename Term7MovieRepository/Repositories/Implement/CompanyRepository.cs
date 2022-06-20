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

        private const string FILTER_BY_NAME = "Name";

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

                string sql = @" SELECT c.Id, c.Name, c.LogoUrl, c.IsActive, c.ManagerId, u.FullName 'ManagerName', u.Email 'ManagerEmail'
                                FROM Companies c LEFT JOIN Users u ON c.ManagerId = u.Id " +
                                
                                GetAdditionFilterQuery(request, FILTER_BY_NAME) +

                             @" ORDER BY c.Id 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(*) 
                                FROM Companies c" +

                                GetAdditionFilterQuery(request, FILTER_BY_NAME) +

                                " ; ";

                string theaterQuery = !request.TheaterIncluded ? "" :

                 @" SELECT Id, Name, Address, CompanyId, ManagerId, Status, Latitude, Longitude
                                         FROM Theaters ";

                object param = new { fetch, offset , request.SearchKey};

                var multiQ = await con.QueryMultipleAsync(sql + count + theaterQuery, param);

                IEnumerable<CompanyDto> list = await multiQ.ReadAsync<CompanyDto>();

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                if (request.TheaterIncluded)
                {
                    IEnumerable<TheaterDto> theaters = await multiQ.ReadAsync<TheaterDto>();

                    foreach (var company in list)
                    {
                        company.Theaters = theaters.Where(t => t.CompanyId == company.Id);
                    }
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
        public async Task<int> UpdateCompany(CompanyUpdateRequest request)
        {
            int count = 0;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    @" UPDATE Companies 
                       SET IsActive = @IsActive " + 

                       (request.Name != null ? " , Name = @Name " : "") +
                       (request.LogoUrl != null ? " , LogoUrl = @LogoUrl " : "") +

                    @" WHERE Id = @Id ; ";

                if (!request.IsActive)
                {
                    sql += @" UPDATE Users
                              SET StatusId = 1 , LockoutEnd = '9999-12-31'
                              WHERE CompanyId = @Id ";
                } else
                {
                    sql += @" UPDATE Users
                              SET StatusId = 2 , LockoutEnd = NULL 
                              WHERE CompanyId = @Id ";
                }

                count = await con.ExecuteAsync(sql, request);

            }

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

            switch(filter)
            {
                case FILTER_BY_NAME:
                    if (!string.IsNullOrEmpty(request.SearchKey))  query = " WHERE c.Name LIKE CONCAT('%', @SearchKey, '%') ";
                    break;
            }

            return query;
        }
    }
}
