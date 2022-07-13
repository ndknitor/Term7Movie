using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TopUpService : ITopUpService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ITopUpHistoryRepository topUpHistoryRepository;

        private const string DESCRIPTION_TOPUP = "Top up";

        public TopUpService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            topUpHistoryRepository = unitOfWork.TopUpHistoryRepository;
        }

        public async Task<ParentResponse> TopUpAsync(TopUpRequest request, long userId)
        {
            TopUpHistory topUpHistory = new TopUpHistory
            {
                Amount = request.Amount,
                Id = Guid.NewGuid(),
                Description = DESCRIPTION_TOPUP,
                RecordDate = DateTime.UtcNow,
                TransactionId = null,
                UserId = userId
            };

            await topUpHistoryRepository.CreateTopUpHistory(topUpHistory);
            
            if (await unitOfWork.CompleteAsync())
            {
                return new ParentResponse
                {
                    Message = Constants.MESSAGE_SUCCESS
                };
            }

            throw new DbOperationException();
        }

        public async Task<ParentResultResponse> GetAllTopUpHistoryAsync(TopUpHistoryFilterRequest request, long userId, string role)
        {
            PagingList<TopUpHistoryDto> pagingList;

            if (Constants.ROLE_CUSTOMER == role)
            {
                request.UserId = userId;
                request.IncludeUser = false;
                request.Email = null;
            }

            pagingList = await topUpHistoryRepository.GetAllAsync(request);

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = pagingList
            };
        }
    }
}
