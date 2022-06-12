using Microsoft.Extensions.Options;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
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
        private readonly ITransactionRepository transactionRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly IPaymentService _paymentService;
        public TransactionService(IUnitOfWork unitOfWork, IPaymentService paymentService)
        {
            _unitOfWork = unitOfWork;
            transactionRepo = _unitOfWork.TransactionRepository;
            ticketRepo = _unitOfWork.TicketRepository;
            _paymentService = paymentService;
        }

        public async Task<TransactionCreateResponse> CreateTransactionAsync(TransactionCreateRequest request, UserDTO user)
        {
            IEnumerable<Ticket> tickets = await ticketRepo.GetTicketByIdListAsync(request.IdList);

            decimal total = tickets.Sum(t => t.SellingPrice);


            // create transaction - pending
            Transaction transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                CustomerId = user.Id,
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

                // create momo payment request
                transaction.Tickets = tickets.ToList();

                var result = await _paymentService.CreateMomoPaymentRequestAynsc(transaction, user);

                if (result == null) throw new DbOperationException("Cannot create momo payment request");

                return new TransactionCreateResponse { 
                    Result = result,
                    Message = Constants.MESSAGE_SUCCESS 
                };
            }

            throw new DbOperationException();
            

            // successs => change transaction success => status add to history o controller khasc

            // add transaction id to ticket
        }

        public async Task ProcessPaymentAsync(MomoIPNRequest ipn) => await _paymentService.ProcessMomoIPNRequestAsync(ipn);
    }
}
