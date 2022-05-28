using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;
using Newtonsoft.Json.Linq;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Response;
using AutoMapper;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Microsoft.Extensions.Options;

namespace Term7MovieService.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly GoogleAuthOption _googleAuthOption;
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        public AuthService(ITokenService tokenService, IUnitOfWork unitOfWork, IOptions<GoogleAuthOption> googleAuthOption, IMapper mapper, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _tokenService = tokenService;
            _googleAuthOption = googleAuthOption.Value;
        }
        public async Task<AuthResponse> GoogleSignInAsync(string idToken)
        {
            UserInfo userInfo;
            AuthResponse response = null;
            using (var client = new HttpClient())
            {
                string jsonString = null;
                string url = _googleAuthOption.TokenUri + Constants.QUESTION_MARK + Constants.GOOGLE_TOKEN_PARAM + idToken;
                var message = await client.GetAsync(url);
                if (message.IsSuccessStatusCode)
                {
                    jsonString = await message.Content.ReadAsStringAsync();
                    userInfo = GetUserInfo(jsonString);
                    User user = await _userManager.FindByLoginAsync(userInfo.ProviderId, userInfo.Uid);
                    
                    if (user == null)
                    {
                        return await RegisterCustomerAsync(userInfo);
                    }

                    return await CreateTokensAsync(user);
                }
                return await Task.FromResult(response);
            }
        }

        public async Task<ExchangeTokenResponse> ExchangeTokenAsync(ExchangeTokenRequest request) => await _tokenService.ExchangeNewAccessTokenAsync(request);


        private UserInfo GetUserInfo(string jsonString)
        {
            JObject jo = JObject.Parse(jsonString);
            UserInfo userInfo = new UserInfo()
            {
                DisplayName = jo[ Constants.GOOGLE_USER_INFO_GIVEN_NAME] + " " + jo[Constants.GOOGLE_USER_INFO_FAMILY_NAME],
                ProviderId = Constants.GOOGLE_USER_INFO_PROVIDER,
                Email = jo[Constants.GOOGLE_USER_INFO_EMAIL] + "",
                PhotoUrl = jo[Constants.GOOGLE_USER_INFO_PICTURE] + "",
                Uid = jo[Constants.GOOGLE_USER_INFO_SUB] + ""
            };
            return userInfo;
        }

        private async Task<AuthResponse> CreateTokensAsync(User user, string roleName = null)
        {
            AuthResponse response = null;

            if (user.StatusId == (int)UserStatusEnum.InActive)
            {
                return response;
            }

            Guid jti = Guid.NewGuid();

            if(roleName == null)
            {
                IList<string> roles = await _userManager.GetRolesAsync(user);
                roleName = roles.FirstOrDefault();
            }
            // create jwt tokens
            response = new AuthResponse
            {
                AccessToken = _tokenService.GenerateAccessToken(user, roleName, jti),
                RefreshToken = await _tokenService.GenerateRefreshTokenAsync(user, jti),
                Message = Constants.MESSAGE_SUCCESS
            };
            return response;
        }

        private async Task<AuthResponse> RegisterCustomerAsync(UserInfo userInfo)
        {
            AuthResponse response = null;

            User user = _mapper.Map<User>(userInfo);
            UserLoginInfo loginInfo = new UserLoginInfo(userInfo.ProviderId, userInfo.Uid, userInfo.ProviderId);

            Role customerRole = new Role()
            {
                Id = (int)RoleEnum.Customer,
                Name = Constants.ROLE_CUSTOMER
            };

            // add user
            var result = await _userManager.CreateAsync(user);

            // add role
            await _userManager.AddToRoleAsync(user, customerRole.Name);

            // add login
            await _userManager.AddLoginAsync(user, loginInfo);

            // create jwt tokens
            response = await CreateTokensAsync(user, customerRole.Name);

            return response;
        }
    }
}
