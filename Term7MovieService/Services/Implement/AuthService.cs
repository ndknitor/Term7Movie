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
using Term7MovieCore.Extensions;
using System.Text;
using System.Net.Mime;

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
                string url = _googleAuthOption.TokenUri;
                var contentObj = new
                {
                    idToken,
                };
                var stringContent = new StringContent(contentObj.ToJson(), Encoding.UTF8, MediaTypeNames.Application.Json);

                var message = await client.PostAsync(url, stringContent);
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
            var users = jo[Constants.USERS][0];
            var providerInfo = users[Constants.PROVIDER_USER_INFO][0];
            UserInfo userInfo = new UserInfo()
            {
                DisplayName = users[Constants.USER_INFO_DISPLAY_NAME].ToString(),
                ProviderId = providerInfo[Constants.PROVIDER_ID].ToString(),
                Email = users[Constants.USER_INFO_EMAIL].ToString(),
                PhotoUrl = users[Constants.USER_INFO_PICTURE].ToString(),
                Uid = providerInfo[Constants.USER_RAW_ID].ToString()
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
