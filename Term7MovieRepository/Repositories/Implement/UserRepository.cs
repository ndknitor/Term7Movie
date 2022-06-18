using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data;

namespace Term7MovieRepository.Repositories.Implement
{
    public class UserRepository : IUserRepository
    {

        private AppDbContext _context;
        private ConnectionOption _connectionOption;

        private const string FILTER_BY_EMAIL = "Email";
        public UserRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }

        public async Task<PagingList<UserDTO>> GetAllUserAsync(UserFilterRequest request)
        {
            PagingList<UserDTO> pagingList = new PagingList<UserDTO>();

            using ( SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1);
                int fetch = request.PageSize;

                string count = @" SELECT COUNT(*)  
                                  FROM Users u LEFT JOIN Companies c ON u.CompanyId = c.Id
                                       JOIN UserRoles ur ON u.Id = ur.UserId
                                  WHERE ur.RoleId != @RoleId " +

                                  GetAdditionUserFilter(request, FILTER_BY_EMAIL) +

                               @" ; ";

                string sql = @" SELECT u.Id, u.FullName, u.Email, u.PictureUrl, u.Point, u.CompanyId, u.StatusId, us.Name 'StatusName', c.Id, c.Name, r.Id, r.Name
                                FROM Users u LEFT JOIN Companies c ON u.CompanyId = c.Id 
                                     JOIN UserRoles ur ON u.Id = ur.UserId
                                     JOIN Roles r ON r.Id = ur.RoleId
                                     JOIN UserStatus us ON u.StatusId = us.Id
                                WHERE ur.RoleId != @RoleId " +

                                GetAdditionUserFilter(request, FILTER_BY_EMAIL) +

                             @" ORDER BY u.Id 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ";


                object param = new { offset, fetch, RoleId = (int)RoleEnum.Admin, Email = request.Email };

                var multiQ = await con.QueryMultipleAsync(count + sql, param);

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                IEnumerable<UserDTO> users = multiQ.Read<UserDTO, CompanyDto, RoleDto, UserDTO>((u, c, r) =>
                {
                    u.Role = r;
                    u.Company = c;
                    return u;
                }, splitOn: "Id");

                pagingList = new PagingList<UserDTO>(request.PageSize, request.Page, users, total);
            }

            return pagingList;
        }

        //Sharingan :D =>>>> Count query thieu where condition roi 
        public async Task<PagingList<UserDTO>> GetAllUserExceptAdaminAsync(UserFilterRequest request)
        {
            PagingList<UserDTO> pagingList = new PagingList<UserDTO>();

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int offset = request.PageSize * (request.Page - 1);
                int fetch = request.PageSize;

                string count = @" SELECT COUNT(*)  
                                  FROM Users u LEFT JOIN Companies c ON u.CompanyId = c.Id ; ";

                string sql = @" SELECT u.Id, u.FullName, u.Email, u.PictureUrl, u.Point, u.CompanyId, u.StatusId, us.Name 'StatusName', c.Id, c.Name, r.Id, r.Name
                                FROM Users u LEFT JOIN Companies c ON u.CompanyId = c.Id 
                                     JOIN UserRoles ur ON u.Id = ur.UserId
                                     JOIN Roles r ON r.Id = ur.RoleId
                                     JOIN UserStatus us ON u.StatusId = us.Id
                                WHERE r.Id != @RoleId
                                ORDER BY u.Id 
                                OFFSET @offset ROWS
                                FETCH NEXT @fetch ROWS ONLY ";


                object param = new { offset, fetch, RoleId = (int)RoleEnum.Admin };

                var multiQ = await con.QueryMultipleAsync(count + sql, param);

                long total = await multiQ.ReadFirstOrDefaultAsync<long>();

                IEnumerable<UserDTO> users = multiQ.Read<UserDTO, CompanyDto, RoleDto, UserDTO>((u, c, r) =>
                {
                    u.Role = r;
                    u.Company = c;
                    return u;
                }, splitOn: "Id");

                pagingList = new PagingList<UserDTO>(request.PageSize, request.Page, users, total);
            }

            return pagingList;
        }

        public async Task<int> GetCompanyIdByManagerId(long managerId)
        {
            int companyId = -1;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT CompanyId " +
                    " FROM Users " +
                    " WHERE Id = @managerId ";
                object param = new { managerId };

                companyId = await con.QueryFirstOrDefaultAsync<int>(sql, param);
            }

            return companyId;
        }
        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }
        public async Task UpdateUserAsync(User userUpdate)
        {
            User user = await _context.Users.FindAsync(userUpdate.Id);
            user.UserName = userUpdate.UserName;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(long id)
        {
            User user = await _context.Users.FindAsync(id);
            user.StatusId = (int) UserStatusEnum.InActive;
        }

        public async Task<long> GetUserIdByUserLoginAsync(UserInfo userInfo)
        {
            var userLogin =  await _context.UserLogins.AsNoTracking()
                .FirstOrDefaultAsync(
                ul => userInfo.ProviderId.Equals(ul.LoginProvider)
                && userInfo.Uid.Equals(ul.ProviderKey));

            if (userLogin == null) return -1;

            return userLogin.UserId;
        }

        public async Task<User> GetUserByIdAsync(long id)
        {
            User user = null;
            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql = 
                    " SELECT Id, Email, FullName, Address, PictureUrl, Point, CompanyId, StatusId " +
                    " FROM Users " +
                    " WHERE Id = @id ";
                var param = new { id };
                user = await con.QueryFirstOrDefaultAsync<User>(sql, param);
            }

            return user;
        }

        public async Task<User> GetUserWithRoleByIdAsync(long id)
        {
            User user = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string userQ =
                    " SELECT Id, Email, FullName, PictureUrl " +
                    " FROM Users " +
                    " WHERE Id = @id ; ";

                string roleQ =
                    " SELECT UserId, RoleId, r.Id, r.Name " +
                    " FROM UserRoles ur JOIN Roles r ON ur.RoleId = r.Id " +
                    " WHERE UserId = @id ; ";
                var param = new { id };

                var multiQ = await con.QueryMultipleAsync(userQ + roleQ, param);

                user = await multiQ.ReadFirstOrDefaultAsync<User>();

                if (user != null) {

                    IEnumerable<UserRole> userRoles = multiQ.Read<UserRole, Role, UserRole>((ur, r) =>
                    {
                        ur.Role = r;
                        return ur;
                    }, splitOn: "Id");

                    user.UserRoles = userRoles.ToList();
                }
            }

            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
            var user = await _context.Users.FirstOrDefaultAsync(a => a.Id == id);
            return user;
        }

        public async Task<int> UpdateUserRole(RoleUpdateRequest request)
        {
            int count = 0;

            using(SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                await con.OpenAsync();
                var transaction = await con.BeginTransactionAsync();
                try
                {
                    string sql =
                    @" 
                       UPDATE Users
                       SET CompanyId = @CompanyId
                       WHERE Id = @UserId ;

                       DELETE FROM UserRoles 
                       WHERE UserId = @UserId ;

                       INSERT INTO UserRoles (UserId, RoleId)
                       VALUES (@UserId, @RoleId); 
                        
                       UPDATE Companies
                       SET ManagerId = @UserId 
                       WHERE Id = @CompanyId AND ManagerId IS NULL ";

                    object param = new { request.UserId, request.CompanyId, RoleId = (int)RoleEnum.Manager };

                    count = await con.ExecuteAsync(sql, param, transaction: transaction);
                    
                    if (count == 4)
                    {
                        transaction.Commit();
                        return count;
                    }

                    throw new BadRequestException(ErrorMessageConstants.ERROR_MESSAGE_INVALID_USER_OR_COMPANY);
                } 
                catch(DbUpdateException e)
                {
                    transaction.Commit();
                    throw e;
                }
            }
        }

        private string GetAdditionUserFilter(UserFilterRequest request, string filter)
        {
            string query = "";

            switch(filter)
            {
                case FILTER_BY_EMAIL:

                    if (!string.IsNullOrEmpty(request.Email))
                    {
                        query = " AND Email LIKE CONCAT('%', @Email, '%') "; // param { Email = "%" + email + "%" de dung LIKE sql}
                    }

                    break;
            }

            return query;
        }
    }
}
