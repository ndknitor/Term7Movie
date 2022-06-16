

using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository userRepository;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            userRepository = _unitOfWork.UserRepository;
        }

        public async Task<UserListResponse> GetAllUserAsync(UserFilterRequest request)
        {
            PagingList<UserDTO> pagingList = await userRepository.GetAllUserAsync(request);

            return new UserListResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Users = pagingList
            };

        }

        public async Task<UserResponse> GetUserFromId(int userid)
        {
            var user = await userRepository.GetUserById(userid);
            if (user == null)
                return new UserResponse { Message = "Can't access to database or user wasn't found." };
            UserDTO dto = new UserDTO();
            dto.FullName = user.FullName;
            dto.Address = user.Address;
            dto.Phone = user.PhoneNumber;
            dto.Email = user.Email;
            dto.PictureUrl = user.PictureUrl;
            dto.Point = user.Point;
            UserResponse response = new UserResponse()
            {
                user = dto,
                Message = "Successful"
            };
            return response;
        }

        public async Task<ParentResponse> UpdateNameForUser(UserRequest request)
        {
            var user = await userRepository.GetUserById(request.UserId);
            if (user == null)
                return new ParentResponse { Message = "Can't access to database or user wasn't found." };
            user.FullName = request.FullName;
            await userRepository.UpdateUserAsync(user);
            return new ParentResponse { Message = "Does it success? I don't know." };
        }
    }
}
