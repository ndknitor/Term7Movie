using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto.Ticket;
using Term7MovieCore.Data.Response;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITicketRepository tiktokRepository;

        public TicketService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            tiktokRepository = _unitOfWork.TicketRepository;
        }

        public async Task<TicketResponse> GetTicketForAShowTime(int showtimeid)
        {
            var rawdata = await tiktokRepository.GetAllTicketByShowtime(showtimeid);
            if (rawdata == null) 
                return new TicketResponse { Message = "Can't access data storage." };
            if (!rawdata.Any())
                return new TicketResponse { Message = "No ticket was found" };
            List<TicketDTO> result = new List<TicketDTO>();
            foreach(var ticket in rawdata)
            {
                TicketDTO dto = new TicketDTO();
                dto.TicketId = ticket.Id;
                dto.SeatId = ticket.SeatId;
                dto.TransactionId = ticket.TransactionId;
                dto.ShowTimeId = ticket.ShowTimeId;
                dto.ShowStartTime = ticket.ShowStartTime;
                dto.OriginalPrice = ticket.OriginalPrice;
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

        public async Task<TicketResponse> GetTicketForATransaction(Guid transactionid)
        {
            var rawdata = await tiktokRepository.GetAllTicketByTransactionId(transactionid);
            if (rawdata == null)
                return new TicketResponse { Message = "Can't access data storage." };
            if(!rawdata.Any())
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
                dto.OriginalPrice = ticket.OriginalPrice;
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

        public async Task<TicketResponse> GetDetailOfATicket(long id)
        {
            var rawdata = await tiktokRepository.GetTicketById(id);
            if (rawdata == null)
                return new TicketResponse { Message = "Can't access database or ticket no longer exists" };
            return new TicketResponse
            {
                Tickets = new List<TicketDTO>
                {
                    new TicketDTO
                    {
                        TicketId = rawdata.Id,
                        SeatId = rawdata.SeatId,
                        TransactionId = rawdata.TransactionId,
                        ShowTimeId = rawdata.ShowTimeId,
                        ShowStartTime = rawdata.ShowStartTime,
                        OriginalPrice = rawdata.OriginalPrice,
                        ReceivePrice = rawdata.ReceivePrice,
                        SellingPrice = rawdata.SellingPrice,
                        StatusId = rawdata.StatusId,
                        LockedTime = rawdata.LockedTime
                    }
                }
            }; //how to C#
        }
    }
}
