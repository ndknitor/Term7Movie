﻿using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Room;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface IRoomRepository
    {
        Task<PagingList<RoomDto>> GetAllRoomByTheaterId(RoomFilterRequest request);
        Task<RoomDto> GetRoomById(int id);
        Task CreateRoom(Room room);
        Task UpdateRoom(Room room);
        Task DeleteRoom(int roomId);
        Task<IEnumerable<RoomDto>> GetRoomByManagerIdAsync(long managerId);
        Task<bool> CheckRoomExist(long managerId, int theaterId, int roomId);
        Task<IEnumerable<RoomNumberDTO>> GetRoomNumberFromTheater(int theaterid);
    }
}
