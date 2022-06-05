using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using Term7MovieCore.Data.Options;

namespace Term7MovieRepository.Repositories.Implement
{
    public class UserRepository : IUserRepository
    {

        private AppDbContext _context;
        private ConnectionOption _connectionOption;
        public UserRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
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
    }
}
