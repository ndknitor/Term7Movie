
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieCore.Extensions;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TokenService : ITokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtOption _jwtOption;
        private readonly IRefreshTokenRepository refreshTokenRepo;
        public TokenService(IUnitOfWork unitOfWork, IOptions<JwtOption> jwtOption)
        {
            _unitOfWork = unitOfWork;
            _jwtOption = jwtOption.Value;
            refreshTokenRepo = unitOfWork.RefreshTokenRepository;
        }

        public string GenerateAccessToken(User user, string roleName, Guid jti)
        {

            List<Claim> claims = GetAccessTokenClaims(user, roleName, jti);

            return GenerateToken(_jwtOption.Key, claims, _jwtOption.ExpiredIn);
        }

        public async Task<string> GenerateRefreshTokenAsync(User user, Guid jti)
        {            

            RefreshToken refreshToken = new RefreshToken()
            {
                ExpiredDate = DateTime.UtcNow.AddSeconds(_jwtOption.RefreshExpiredIn),
                Jti = jti,
                UserId = user.Id
            };

            List<Claim> claims = GetRefreshTokenClaims(refreshToken);

            refreshToken.Value = GenerateToken(_jwtOption.Key, claims, _jwtOption.RefreshExpiredIn);

            await refreshTokenRepo.CreateRefreshTokenAsync(refreshToken);

            await _unitOfWork.CompleteAsync();

            return refreshToken.Value;
        }

        public async Task<ExchangeTokenResponse> ExchangeNewAccessTokenAsync(ExchangeTokenRequest request)
        {
            ExchangeTokenResponse response = new ExchangeTokenResponse();

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            JwtSecurityToken token;

            if (!handler.CanReadToken(request.RefreshToken))
            {
                {
                    response.Message = Constants.MESSAGE_INVALID_REFRESH_TOKEN;
                    return response;
                }
            }

            token = handler.ReadJwtToken(request.RefreshToken);

            int? exp = token.Payload.Exp;
            string hash = token.Claims.FindFirstValue(ClaimTypes.Hash);
            string userId = token.Claims.FindFirstValue(ClaimTypes.NameIdentifier);
            string jti = token.Claims.FindFirstValue(JwtRegisteredClaimNames.Jti);

            IUserRepository userRepo = _unitOfWork.UserRepository;
            User user;

            if (!IsRefreshTokenValid(exp, hash, userId, jti))
            {
                response.Message = Constants.MESSAGE_INVALID_REFRESH_TOKEN;
                return response;
            }

            RefreshToken refreshToken = await refreshTokenRepo.GetRefreshTokenByJtiAsync(jti);

            if (!IsRefreshTokenExist(request, refreshToken))
            {
                response.Message = Constants.MESSAGE_INVALID_REFRESH_TOKEN;
                return response;
            }

            if (refreshToken.ExpiredDate < DateTime.UtcNow)
            {
                response.Message = Constants.MESSAGE_EXIRED_REFRESH_TOKEN;
                return response;
            }

            user = await userRepo.GetUserWithRoleByIdAsync(Convert.ToInt64(userId));
            string userRoleName = user.UserRoles.FirstOrDefault()?.Role.Name;

            response.AccessToken = GenerateAccessToken(user, userRoleName, Guid.Parse(jti));
            response.Message = Constants.MESSAGE_SUCCESS;
            return response;
        }

        private bool IsRefreshTokenExist(ExchangeTokenRequest request, RefreshToken refreshToken)
        {
            // not found refresh token by jti
            if (refreshToken == null)
            {
                return false;
            }

            // value of refresh token is not match
            if (!refreshToken.Value.Equals(request.RefreshToken))
            {
                return false;
            }

            return true;
        }

        private bool IsAccessTokenExpired(int exp)
        {
            return Constants.JSON_START_DATE.AddSeconds(exp) < DateTime.UtcNow;
        }

        private bool IsRefreshTokenValid(int? exp, string roleName, string userId, string jti)
        {
            return exp != null && roleName != null && userId != null && jti != null;
        }

        private List<Claim> GetAccessTokenClaims(User user, string roleName, Guid jti)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, roleName),
                new Claim(JwtRegisteredClaimNames.Jti, jti.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(Constants.JWT_CLAIM_PICTURE, user.PictureUrl)
             };
        }

        private List<Claim> GetRefreshTokenClaims(RefreshToken refreshToken)
        {
            return new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, refreshToken.UserId.ToString()),
                new Claim(ClaimTypes.Hash, (Guid.NewGuid() + Guid.NewGuid().ToString()).ToBase64String()),
                new Claim(JwtRegisteredClaimNames.Jti, refreshToken.Jti.ToString())
            };
        }
        private string GenerateToken(string key, List<Claim> claims, double expiredTime)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            SigningCredentials credential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            DateTime expired = DateTime.UtcNow.AddSeconds(expiredTime);

            JwtSecurityToken token = new JwtSecurityToken(claims: claims, expires: expired, signingCredentials: credential);

            return tokenHandler.WriteToken(token);
        }
    }
}
