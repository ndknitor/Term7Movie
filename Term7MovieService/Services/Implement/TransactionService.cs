using Microsoft.Extensions.Options;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly MomoOption momoOption;
        private readonly ITransactionRepository transactionRepo;
        private readonly ITransactionHistoryRepository transactionHistoryRepo;
        private readonly ITicketRepository ticketRepo;

        public TransactionService(IUnitOfWork unitOfWork, IOptions<MomoOption> option)
        {
            _unitOfWork = unitOfWork;
            momoOption = option.Value;
            transactionRepo = _unitOfWork.TransactionRepository;
            transactionHistoryRepo = _unitOfWork.TransactionHistoryRepository;
            ticketRepo = _unitOfWork.TicketRepository;
        }

        public async Task<ParentResponse> CreateTransactionAsync(TransactionCreateRequest request, long customerId)
        {
            IEnumerable<Ticket> list = await ticketRepo.GetTicketByIdListAsync(request.IdList);

            decimal total = list.Sum(t => t.SellingPrice);


            // create transaction - pending
            Transaction transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = customerId,
                PurchasedDate = DateTime.UtcNow,
                StatusId = (int)TransactionStatusEnum.Pending,
                Total = total,
            };

            await transactionRepo.CreateTransaction(transaction);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();

                // lock ticket
                await ticketRepo.LockTicketAsync(request.IdList);

                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            throw new DbOperationException();
            // create momo payment request

            // successs => change transaction success => status add to history o controller khasc

            // add transaction id to ticket
        }
    }
}
