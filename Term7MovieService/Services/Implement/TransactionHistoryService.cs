using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TransactionHistoryService : ITransactionHistoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionHistoryRepository transactionHistoryRepo;

        public TransactionHistoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            transactionHistoryRepo = _unitOfWork.TransactionHistoryRepository;

        }

        public async Task<TransactionHistoryListResponse> GetTransactionListHistoryAynsc(ParentFilterRequest request, string role, long userId)
        {
            PagingList<TransactionHistory> list = null;
            switch (role)
            {
                case Constants.ROLE_ADMIN:
                    list = await transactionHistoryRepo.GetAllTransactionHistory(request);
                    break;
                case Constants.ROLE_MANAGER:
                    list = await transactionHistoryRepo.GetAllTransactionHistoryByCompanyId(request, userId);
                    break;
                case Constants.ROLE_CUSTOMER:
                    list = await transactionHistoryRepo.GetAllTransactionHistoryByCustomerId(request, userId);
                    break;
            }
            return new TransactionHistoryListResponse 
            { 
                Message = Constants.MESSAGE_SUCCESS,
                History = list
            };
        }
    }
}
