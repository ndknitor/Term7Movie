using AutoMapper;
using System;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;
using Term7MovieService.Services.Interface;

namespace Term7MovieService.Services.Implement
{
    public class RoomService : IRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoomRepository roomRepo;
        private readonly IMapper _mapper;
        public RoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            roomRepo = unitOfWork.RoomRepository;
            _mapper = mapper;
        }
        public async Task<RoomResponse> GetRoomDetail(int roomId)
        {
            RoomDto dto = await roomRepo.GetRoomById(roomId);

            if (dto == null)
            {
                return null;
            }

            return new RoomResponse()
            {
                Message = Constants.MESSAGE_SUCCESS,
                Room = dto
            };
        }

        public async Task<TheaterRoomsResponse> GetRoomsByTheaterId(int theaterId)
        {
            IEnumerable<RoomDto> rooms = await roomRepo.GetAllRoomByTheaterId(theaterId);

            if ((rooms as List<RoomDto>).Count == 0)
            {
                return null;
            }

            return new TheaterRoomsResponse()
            {
                Message = Constants.MESSAGE_SUCCESS,
                TheaterId = theaterId,
                Rooms = rooms
            };
        }

        public async Task<ParentResponse> CreateRoom(RoomCreateRequest request)
        {
            Room room = _mapper.Map<Room>(request);
            await roomRepo.CreateRoom(room);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }

            return null;
        }

        public async Task<ParentResponse> UpdateRoom(RoomUpdateRequest request)
        {
            Room room = _mapper.Map<Room>(request);
            await roomRepo.UpdateRoom(room);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }
            return null;
        }

        public async Task<ParentResponse> DeleteRoom(int id)
        {
            await roomRepo.DeleteRoom(id);

            if (_unitOfWork.HasChange())
            {
                await _unitOfWork.CompleteAsync();
                return new ParentResponse { Message = Constants.MESSAGE_SUCCESS };
            }
            return null;
        }
    }
}
