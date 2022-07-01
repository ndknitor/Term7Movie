using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Ticket;
using Term7MovieCore.Data.Exceptions;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Cache.Interface;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketRepository ticketRepository;
        private readonly ICacheProvider cacheProvider;

        public TicketService(IUnitOfWork unitOfWork, ICacheProvider cacheProvider)
        {
            _unitOfWork = unitOfWork;
            ticketRepository = _unitOfWork.TicketRepository;
            this.cacheProvider = cacheProvider;
        }

        public async Task<ParentResultResponse> GetTicketAsync(TicketFilterRequest request)
        {
            PagingList<TicketDto> list = await ticketRepository.GetAllTicketAsync(request);

            return new ParentResultResponse
            {
                Result = list,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<TicketResponse> GetTicketForSomething(TicketRequest request)
        {
            if (!request.TransactionId.HasValue && !request.ShowTimeId.HasValue)
                return new TicketResponse { Message = "Parameter is missing, need at least one parameter" };
            if (request.TransactionId.HasValue && request.ShowTimeId.HasValue)
                return new TicketResponse { Message = "One parameter at a time only" };
            if (request.TransactionId.HasValue)
            {
                Guid dummy = request.TransactionId.Value;
                return await GetTicketForATransaction(dummy);
            }
            if (request.ShowTimeId.HasValue)
            {
                int id = request.ShowTimeId.Value;
                return await GetTicketForAShowTime(id);
            }
            throw new Exception("MEDIC MEDICCCCCCCCCCC");
        }

        private async Task<TicketResponse> GetTicketForAShowTime(int showtimeid)
        {
            var rawdata = await ticketRepository.GetAllTicketByShowtime(showtimeid);
            if (rawdata == null)
                return new TicketResponse { Message = "Can't access data storage." };
            if (!rawdata.Any())
                return new TicketResponse { Message = "No ticket was found" };
            List<TicketDTO> result = new List<TicketDTO>();
            foreach (var ticket in rawdata)
            {
                TicketDTO dto = new TicketDTO();
                dto.TicketId = ticket.Id;
                dto.SeatId = ticket.SeatId;
                dto.TransactionId = ticket.TransactionId;
                dto.ShowTimeId = ticket.ShowTimeId;
                dto.ShowStartTime = ticket.ShowStartTime;
                //dto.OriginalPrice = ticket.OriginalPrice;
                dto.ReceivePrice = ticket.ReceivePrice;
                dto.SellingPrice = ticket.SellingPrice;
                dto.StatusId = ticket.StatusId;
                dto.LockedTime = ticket.LockedTime;
                result.Add(dto);
            }
            return new TicketResponse
            {
                Message = "Successful",
                Tickets = result
            };
        }

        private async Task<TicketResponse> GetTicketForATransaction(Guid transactionid)
        {
            var rawdata = await ticketRepository.GetAllTicketByTransactionId(transactionid);
            if (rawdata == null)
                return new TicketResponse { Message = "Can't access data storage." };
            if (!rawdata.Any())
                return new TicketResponse { Message = "No ticket was found" };
            List<TicketDTO> result = new List<TicketDTO>();
            foreach (var ticket in rawdata)
            {
                TicketDTO dto = new TicketDTO();
                dto.TicketId = ticket.Id;
                dto.SeatId = ticket.SeatId;
                dto.TransactionId = ticket.TransactionId;
                dto.ShowTimeId = ticket.ShowTimeId;
                dto.ShowStartTime = ticket.ShowStartTime;
                //dto.OriginalPrice = ticket.OriginalPrice;
                dto.ReceivePrice = ticket.ReceivePrice;
                dto.SellingPrice = ticket.SellingPrice;
                dto.StatusId = ticket.StatusId;
                dto.LockedTime = ticket.LockedTime;
                result.Add(dto);
            }
            return new TicketResponse
            {
                Message = "Successful",
                Tickets = result
            };
        }

        public async Task<ParentResultResponse> GetTicketDetail(long id, long showtimeId, string role)
        {
            TicketDto ticket;
            switch(role)
            {
                case Constants.ROLE_CUSTOMER:
                    string showtimeKey = await cacheProvider.GetHashFieldValueAsync<string>(Constants.REDIS_KEY_SHOWTIME_TICKET, Constants.REDIS_KEY_SHOWTIME_TICKET + "_" + showtimeId);

                    if (showtimeKey != null)
                    {
                        IEnumerable<TicketDto> tickets = await cacheProvider.GetValueAsync<IEnumerable<TicketDto>>(showtimeKey);
                        if (tickets == null || !tickets.Any()) throw new DbNotFoundException();

                        ticket = tickets.FirstOrDefault(t => t.Id == id);
                    } 
                    else
                    {
                        ticket = await ticketRepository.GetTicketById(id, isNotShowed: true);
                    }
                    break;
                case Constants.ROLE_ADMIN:
                case Constants.ROLE_MANAGER:
                    ticket = await ticketRepository.GetTicketById(id, isNotShowed: false);
                    break;
                default: 
                    ticket = null; 
                    break;
            }

            if (ticket == null) throw new DbNotFoundException();

            return new ParentResultResponse
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = ticket
            };
        }

        public async Task<ParentResponse> CreateTicketAsync(TicketListCreateRequest request)
        {
            var ticketRepo = ticketRepository;

            int count = await ticketRepo.CreateTicketAsync(request);

            if (count > 0)
            {
                return new ParentResponse
                {
                    Message = Constants.MESSAGE_SUCCESS
                };
            }

            throw new BadRequestException("Invalid Seat Id or ShowtimeTicketTypeId");
        }
    }
}