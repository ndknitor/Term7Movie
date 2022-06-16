
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface IUserService
    {
        Task<UserListResponse> GetAllUserAsync(UserFilterRequest request);
        Task<UserResponse> GetUserFromId(int userid);

        Task<ParentResponse> UpdateNameForUser(UserRequest request);
    }
}
