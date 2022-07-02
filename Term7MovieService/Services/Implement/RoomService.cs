using AutoMapper;
using System;
using Term7MovieCore.Data;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;
using Term7MovieCore.Data.Response.Room;
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

        public async Task<ParentResultResponse> GetRoomsByTheaterId(RoomFilterRequest request)
        {
            PagingList<RoomDto> rooms = await roomRepo.GetAllRoomByTheaterId(request);

            return new ParentResultResponse()
            {
                Message = Constants.MESSAGE_SUCCESS,
                Result = rooms
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

        public async Task<RoomNumberResponse> GetRoomNumberFromTheater(int theaterid)
        {
            try
            {
                var rawdata = await roomRepo.GetRoomNumberFromTheater(theaterid);
                if (rawdata == null)
                    return new RoomNumberResponse { Message = "Database sập rồi" };
                return new RoomNumberResponse
                {
                    Message = Constants.MESSAGE_SUCCESS,
                    RoomNumbers = rawdata
                };

            }
            catch(Exception ex)
            {
                if (ex.Message == "EMPTYDATA")
                    return new RoomNumberResponse { Message = "There is no room for this theater" };
                throw new Exception(ex.Message);
            }
        }
    }
}
