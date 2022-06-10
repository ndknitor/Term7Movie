using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data.Dto.Ticket;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
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

        public async Task<TicketResponse> GetTicketForSomething(TicketRequest request)
        {
            if(!request.TransactionId.HasValue && !request.ShowTimeId.HasValue)
                return new TicketResponse { Message = "Parameter is missing, need at least one parameter" };
            if (request.TransactionId.HasValue && request.ShowTimeId.HasValue)
                return new TicketResponse { Message = "One parameter at a time only" };
            if (request.TransactionId.HasValue)
            {
                Guid dummy = request.TransactionId.Value;
                return await GetTicketForATransaction(dummy);
            }
            if(request.ShowTimeId.HasValue)
            {
                int id = request.ShowTimeId.Value;
                return await GetTicketForAShowTime(id);
            }
            throw new Exception("MEDIC MEDICCCCCCCCCCC");
        }

        private async Task<TicketResponse> GetTicketForAShowTime(int showtimeid)
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

        private async Task<TicketResponse> GetTicketForATransaction(Guid transactionid)
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

        public async Task<ParentResponse> CreateTicket(TicketCreateRequest request)
        {
            try
            {
                Ticket tiktok = new Ticket();
                tiktok.SeatId = request.SeatId;
                tiktok.TransactionId = request.TransactionId;
                tiktok.ShowTimeId = request.ShowTimeId;
                tiktok.ShowStartTime = request.ShowStartTime;
                tiktok.OriginalPrice = request.OriginalPrice;
                tiktok.ReceivePrice = request.ReceivePrice;
                tiktok.SellingPrice = request.SellingPrice;
                tiktok.StatusId = request.StatusId;
                tiktok.LockedTime = request.LockedTime;
                await tiktokRepository.CreateTicket(tiktok);
                return new ParentResponse { Message = "A ticket has created" };
            }
            catch(Exception ex)
            {
                if(ex.Message == "DBCONNECT")
                {
                    return new ParentResponse { Message = "Can't access database" };
                }
                throw new Exception(ex.Message);
            }
        }

        public async Task<ParentResponse> CreateALotOfTicket(TicketCreateRequest[] request)
        {//tập trung làm cái khác quan trọng hơn thay vì cache lỗi
            List<Ticket> tiktokList = new List<Ticket>();
            foreach(var ticket in request)
            {
                Ticket tiktok = new Ticket();
                tiktok.SeatId = ticket.SeatId;
                tiktok.TransactionId = ticket.TransactionId;
                tiktok.ShowTimeId = ticket.ShowTimeId;
                tiktok.ShowStartTime = ticket.ShowStartTime;
                tiktok.OriginalPrice = ticket.OriginalPrice;
                tiktok.ReceivePrice = ticket.ReceivePrice;
                tiktok.SellingPrice = ticket.SellingPrice;
                tiktok.StatusId = ticket.StatusId;
                tiktok.LockedTime = ticket.LockedTime;
                tiktokList.Add(tiktok);
            }
            try
            {
                await tiktokRepository.CreateTicket(tiktokList);
                return new ParentResponse { Message = "A lot of ticket has created but don't know if there is any errors :v" };
#warning có time thì nên response cái list status add thành công cho từng ticket huhu tôi lười
            }
            catch (Exception ex)
            {
                if (ex.Message == "DBCONNECT")
                {
                    return new ParentResponse { Message = "Can't access database" };
                }
                throw new Exception(ex.Message);
            }


        }
    }
}
