using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Term7MovieApi.Extensions;
using Term7MovieApi.Requirements;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieApi.Handlers
{
    public class GeneralAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private ILogger<GeneralAuthorizationHandler> _logger;
        public GeneralAuthorizationHandler(ILogger<GeneralAuthorizationHandler>  logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            IEnumerable<Claim> claims = context.User.Claims;

            var pendingRequirements = context.PendingRequirements.ToList();
            foreach (var requirement in pendingRequirements)
            {
                switch(requirement)
                {
                    case RoomWithSameTheaterRequirement:

                        var httpContext = context.Resource as HttpContext;

                        RoomCreateRequest request = await httpContext.Request.ToObjectAsync<RoomCreateRequest>();

                        bool valid = await IsManagerRequestValid(claims, request);
                        
                        if(valid) context.Succeed(requirement);

                        break;
                }
            }
        }

        //protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, TheaterManagerRequirement requirement, RoomCreateRequest resource)
        //{
        //    var claims = context.User.Claims;
        //
        //    bool isValid = await IsManagerRequestValid(claims, resource);
        //
        //     if (isValid)
        //    {
        //        context.Succeed(requirement);
        //    }
        //}

        private async Task<bool> IsManagerRequestValid(IEnumerable<Claim> claims, RoomCreateRequest resource)
        {
            long managerId = Convert.ToInt64(claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            ITheaterRepository theaterRepo = _unitOfWork.TheaterRepository;

            var theaters = await theaterRepo.GetTheaterByManagerIdAsync(managerId);

            var theater = theaters.FirstOrDefault(t => t.Id == resource.TheaterId);

            if (theater == null) return false;

            return true;
        }
    }
}
