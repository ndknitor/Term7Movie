﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Security.Claims;
using Term7MovieApi.Extensions;
using Term7MovieApi.Requirements;
using Term7MovieApi.Requirements.RoomRequirement;
using Term7MovieApi.Requirements.ShowtimeRequirement;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Extensions;
using Term7MovieCore.Data.Request;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieApi.Handlers
{
    public class GeneralAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GeneralAuthorizationHandler> _logger;
        public GeneralAuthorizationHandler(ILogger<GeneralAuthorizationHandler>  logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            IEnumerable<Claim> claims = context.User.Claims;

            var httpContext = context.Resource as HttpContext;

            var pendingRequirements = context.PendingRequirements.ToList();
            foreach (var requirement in pendingRequirements)
            {
                switch(requirement)
                {
                    case CreateRoomWithSameTheaterRequirement:

                        RoomCreateRequest roomCreateReq = await httpContext.Request.ToObjectAsync<RoomCreateRequest>();

                        if(await IsManagerCreateRoomRequestValid(claims, roomCreateReq)) context.Succeed(requirement);
                        break;

                    case UpdateRoomWithSameTheaterRequirement:

                        RoomUpdateRequest roomUpdateReq = await httpContext.Request.ToObjectAsync<RoomUpdateRequest>();

                        if (await IsManagerUpdateRoomRequestValid(claims, roomUpdateReq)) context.Succeed(requirement);
                        break;

                    case DeleteRoomWithSameTheaterRequirement:

                        var roomId = httpContext.Request.RouteValues["roomId"];

                        if (await IsManagerUpdateRoomRequestValid(claims, new RoomUpdateRequest { Id = Convert.ToInt32(roomId)})) context.Succeed(requirement);

                        break;

                    case CreateTransactionTicketSameShowtimeRequirement:

                        TransactionCreateRequest transactionCreateRequest = await httpContext.Request.ToObjectAsync<TransactionCreateRequest>();

                        if (await IsTransactionCreateRequestValid(claims, transactionCreateRequest)) context.Succeed(requirement);

                        break;

                    case CreateShowtimeForSameManagerRequirement:

                        ShowtimeCreateRequest showtimeCreateRequest = await httpContext.Request.ToObjectAsync<ShowtimeCreateRequest>();

                        if (await CanManagerCreateShowtime(claims, showtimeCreateRequest)) context.Succeed(requirement);

                        break;

                    case CompanyFilterRequirement:

                        if (IsCompanyFilterRequestValid(claims, httpContext.Request)) context.Succeed(requirement);
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

        private async Task<bool> IsManagerCreateRoomRequestValid(IEnumerable<Claim> claims, RoomCreateRequest resource)
        {
            long managerId = Convert.ToInt64(claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            ITheaterRepository theaterRepo = _unitOfWork.TheaterRepository;

            var theaters = await theaterRepo.GetTheaterByManagerIdAsync(managerId);

            var theater = theaters.FirstOrDefault(t => t.Id == resource.TheaterId);

            if (theater == null) return false;

            return true;
        }

        private async Task<bool> IsManagerUpdateRoomRequestValid(IEnumerable<Claim> claims, RoomUpdateRequest resource)
        {
            IRoomRepository theaterRepo = _unitOfWork.RoomRepository;

            long managerId = Convert.ToInt64(claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));

            var rooms = await theaterRepo.GetRoomByManagerIdAsync(managerId);

            var room = rooms.FirstOrDefault(r => r.Id == resource.Id);

            if (room == null) return false;

            return true;
        }

        private async Task<bool> IsTransactionCreateRequestValid(IEnumerable<Claim> claims, TransactionCreateRequest resource)
        {
            ITicketRepository ticketRepository = _unitOfWork.TicketRepository;

            return await ticketRepository.IsTicketInShowtimeValid(resource.ShowtimeId, resource.IdList);
        }

        private async Task<bool> IsShowtimeNotOverlapValid<T>( T resource)
        {
            bool valid = false;
            switch(resource)
            {
                case ShowtimeCreateRequest:
                    valid = await _unitOfWork.ShowtimeRepository.IsShowtimeNotOverlap(resource as ShowtimeCreateRequest);
                    break;
            }

            return valid;
        }

        private async Task<bool> CanManagerCreateShowtime<T>(IEnumerable<Claim> claims, T resource)         
        {
            bool valid = false;
            long managerId = Convert.ToInt64(claims.FindFirstValue(Constants.JWT_CLAIM_USER_ID));
            switch(resource)
            {
                case ShowtimeCreateRequest:
                    valid = await _unitOfWork.ShowtimeRepository.CanManagerCreateShowtime(resource as ShowtimeCreateRequest, managerId);
                    break;
            }

            return valid;
        }

        private bool IsCompanyFilterRequestValid(IEnumerable<Claim> claims, HttpRequest request)
        {
            var filterRequest = new CompanyFilterRequest();

            string role = claims.FindFirstValue(Constants.JWT_CLAIM_ROLE);

            bool withNoManager;

            if (!bool.TryParse(request.Query[nameof(filterRequest.WithNoManager)], out withNoManager)) return true;

            if (!Constants.ROLE_ADMIN.Equals(role) && withNoManager)
            {
                return false;
            }

            return true;
        }
    }
}
