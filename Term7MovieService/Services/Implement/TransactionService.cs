using AutoMapper;
using Microsoft.Extensions.Options;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Enum;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieCore.Extensions;
using Term7MovieRepository.Cache.Interface;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository transactionRepo;
        private readonly ITransactionHistoryRepository transactionHistoryRepo;
        private readonly ITicketRepository ticketRepo;
        private readonly IPaymentService _paymentService;
        private readonly IMapper mapper;
        private readonly ICacheProvider cacheProvider;

        private object lockObject = new object();

        public TransactionService(IUnitOfWork unitOfWork, IPaymentService paymentService, IMapper mapper, ICacheProvider cacheProvider)
        {
            _unitOfWork = unitOfWork;
            transactionRepo = _unitOfWork.TransactionRepository;
            ticketRepo = _unitOfWork.TicketRepository;
            _paymentService = paymentService;
            this.mapper = mapper;
            this.cacheProvider = cacheProvider;
            transactionHistoryRepo = _unitOfWork.TransactionHistoryRepository;
        }

        public TransactionCreateResponse CreateTransaction(TransactionCreateRequest request, UserDTO user)
        {
            lock(lockObject)
            {
                string showtimeTicketKey = Constants.REDIS_KEY_SHOWTIME_TICKET + "_" + request.ShowtimeId;

                if (!cacheProvider.IsHashExist(Constants.REDIS_KEY_SHOWTIME_TICKET, request.ShowtimeId.ToString())) throw new BadRequestException("Showtime not exist");

                IEnumerable<TicketDto> tickets = cacheProvider.GetValue<IEnumerable<TicketDto>>(showtimeTicketKey);

                if (tickets == null || !tickets.Any()) throw new BadRequestException("No ticket available");

                decimal total = 0;

                foreach(long ticketId in request.TicketIdList)
                {
                    TicketDto ticket = tickets.Where(t => t.Id == ticketId).FirstOrDefault();

                    if (ticket == null || ticket.LockedTime > DateTime.UtcNow)
                    {
                        throw new BadRequestException($"Ticket with id: {ticketId} is not available");
                    }

                    total += ticket.SellingPrice;
                    ticket.LockedTime = DateTime.UtcNow.AddMinutes(Constants.LOCK_TICKET_IN_MINUTE).AddSeconds(10);
                    ticket.TransactionId = request.TransactionId;
                }

                TicketDto first = tickets.First();

                Transaction transaction = new Transaction
                {
                    CustomerId = user.Id,
                    Id = request.TransactionId,
                    PurchasedDate = DateTime.UtcNow,
                    StatusId = (int)TransactionStatusEnum.Pending,
                    TheaterId = first.Showtime.TheaterId,
                    ValidUntil = DateTime.UtcNow.AddMinutes(Constants.LOCK_TICKET_IN_MINUTE),
                    Total = total,
                    ShowtimeId = request.ShowtimeId
                };

                transactionRepo.CreateTransaction(transaction);

                if (_unitOfWork.Complete())
                {
                    cacheProvider.Expire = first.ShowStartTime - DateTime.UtcNow;
                    if (cacheProvider.Expire > TimeSpan.Zero)
                    {
                        cacheProvider.SetValue(showtimeTicketKey, tickets);
                    }

                    ticketRepo.LockTicket(request.TicketIdList);

                    return new TransactionCreateResponse
                    {
                        Message = Constants.MESSAGE_SUCCESS
                    };
                }
                     
                throw new DbOperationException();
            }
        }

        public async Task<ParentResponse> CheckPaymentStatus(Guid transactionId, long userId)
        {
            TransactionDto transaction = await transactionRepo.GetTransactionInfoByIdAsync(transactionId);

            if (transaction == null) throw new DbNotFoundException();

            if (transaction.CustomerId != userId) throw new DbForbiddenException();

            if (transaction.ValidUntil < DateTime.UtcNow) throw new BadRequestException("Transaction Expired");

            //int statusId = await _paymentService.CheckMomoPayment(transaction);

            //if (statusId != (int)TransactionStatusEnum.Successful)
            //{
            //    throw new BadRequestException("Transaction failed");
            //}

            string showtimeTicketKey = Constants.REDIS_KEY_SHOWTIME_TICKET + "_" + transaction.ShowtimeId;

            List<TicketDto> tickets = cacheProvider.GetValue<IEnumerable<TicketDto>>(showtimeTicketKey).ToList();

            List<long> boughtTicket = new List<long>();

            foreach (TicketDto ticket in tickets)
            {
                if (ticket.TransactionId == transactionId)
                {
                    boughtTicket.Add(ticket.Id);
                    tickets.Remove(ticket);
                }
            }

            TicketDto first = tickets.FirstOrDefault();

            if (first != null && first.ShowStartTime < DateTime.UtcNow)
            {
                cacheProvider.Expire = DateTime.UtcNow - first.ShowStartTime;
                await cacheProvider.SetValueAsync(showtimeTicketKey, tickets);
            }

            await ticketRepo.BuyTicket(transactionId, boughtTicket);

            await transactionHistoryRepo.CreateTransactionHistory(boughtTicket);

            await transactionRepo.UpdateTransaction(transactionId, (int)TransactionStatusEnum.Successful, 0);

            return new ParentResponse
            {
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<ParentResultResponse> GetAllTransactionAsync(TransactionFilterRequest request, long userId, string roleId)
        {
            var result = await transactionRepo.GetAllTransactionAsync(request, userId, roleId);

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = result
            };
        }

        public async Task<ParentResultResponse> GetTransactionByIdAsync(Guid transactionId)
        {
            var result = await transactionRepo.GetTransactionByIdAsync(transactionId);

            if (result.StatusId == (int)TransactionStatusEnum.Successful) result.QRCodeUrl = result.ToJson().ToBase64String();

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = result
            };
        }

        public async Task ProcessPaymentAsync(MomoIPNRequest ipn) => await _paymentService.ProcessMomoIPNRequestAsync(ipn);
    }
}
