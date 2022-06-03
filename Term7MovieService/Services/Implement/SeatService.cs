using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISeatRepository seatRepo;
        private readonly IMapper _mapper;
        public SeatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            seatRepo = _unitOfWork.SeatRepository;
            _mapper = mapper;
        }

        public async Task<SeatResponse> GetSeatById(long id)
        {
            SeatDto seat = await seatRepo.GetSeatById(id);

            if (seat == null) return null;

            return new SeatResponse
            {
                Seat = seat,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<SeatListResponse> GetRoomSeats(int roomId)
        {
            IEnumerable<SeatDto> seats = await seatRepo.GetRoomSeats(roomId);

            return new SeatListResponse
            {
                Seats = seats,
                Message = Constants.MESSAGE_SUCCESS
            };
        }

        public async Task<ParentResponse> CreateSeat(IEnumerable<SeatCreateRequest> request)
        {
            IEnumerable<Seat> seats = _mapper.Map<IEnumerable<Seat>>(request);
            await seatRepo.CreateSeat(seats);

            if (!_unitOfWork.HasChange())
            {
                return null;
            }

            await _unitOfWork.CompleteAsync();
            return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
        }

        public async Task<ParentResponse> UpdateSeat(SeatUpdateRequest request)
        {
            Seat seat = _mapper.Map<Seat>(request);
            await seatRepo.UpdateSeat(seat);

            if (!_unitOfWork.HasChange())
            {
                return null;
            }

            await _unitOfWork.CompleteAsync();
            return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
        }

        public async Task<ParentResponse> DeleteSeat(long id)
        {
            await seatRepo.DeleteSeat(id);

            if (!_unitOfWork.HasChange())
            {
                return null;
            }

            await _unitOfWork.CompleteAsync();
            return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
        }
    }
}
