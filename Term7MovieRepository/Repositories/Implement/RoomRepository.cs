﻿using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Dto.Room;
using Term7MovieCore.Data.Options;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;
using Term7MovieRepository.Repositories.Interfaces;

namespace Term7MovieRepository.Repositories.Implement
{
    public class RoomRepository : IRoomRepository
    {
        private readonly AppDbContext _context;
        private readonly ConnectionOption _connectionOption;
        public RoomRepository(AppDbContext context, ConnectionOption connectionOption)
        {
            _context = context;
            _connectionOption = connectionOption;
        }
        public async Task<PagingList<RoomDto>> GetAllRoomByTheaterId(RoomFilterRequest request)
        {
            PagingList<RoomDto> list;

            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                int fetch = request.PageSize;
                int offset = request.PageSize*(request.Page - 1);
                string sql = 
                   @" SELECT Id, No, TheaterId, NumberOfRow, NumberOfColumn, Status
                       FROM Rooms
                       WHERE TheaterId = @TheaterId 
                       ORDER BY Id 
                       OFFSET @offset ROWS 
                       FETCH NEXT @fetch ROWS ONLY ; ";

                string count = @" SELECT COUNT(*) 
                                  FROM Rooms 
                                  WHERE TheaterId = @TheaterId ; ";

                var param = new { offset, fetch, request.TheaterId };

                var multiQ = await con.QueryMultipleAsync(sql + count, param);

                IEnumerable<RoomDto> rooms = await multiQ.ReadAsync<RoomDto>();

                int total = await multiQ.ReadFirstOrDefaultAsync<int>();

                list = new PagingList<RoomDto>(request.PageSize, request.Page, rooms, total);
            }

            return list;
        }
        public async Task<RoomDto> GetRoomById(int id)
        {
            RoomDto room = null;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string roomSql =
                    " SELECT Id, No, TheaterId, NumberOfRow, NumberOfColumn, Status " +
                    " FROM Rooms " +
                    " WHERE Id = @id AND Status = 1 ; ";

                string seatSql =
                    " SELECT s.Id, s.Name, s.RoomId, s.ColumnPos, s.RowPos, s.SeatTypeId, st.Id, st.Name " +
                    " FROM Seats s JOIN SeatTypes st ON s.SeatTypeId = st.Id " +
                    " WHERE s.RoomId = @id ";

                var param = new { id };

                var multiQ = await con.QueryMultipleAsync(roomSql + seatSql, param);

                room = await multiQ.ReadFirstOrDefaultAsync<RoomDto>();

                if (room == null) return null;

                List<SeatDto> seats = multiQ.Read<SeatDto, SeatTypeDto, SeatDto>((s, st) => 
                {
                    s.SeatType = st;
                    return s;
                },splitOn: "Id").ToList();

                room.SeatDtos = seats;
            }
            return room;
        }
        public async Task CreateRoom(Room room)
        {
            await _context.AddAsync(room);
        }
        public async Task UpdateRoom(Room room)
        {
            Room dbRoom = await  _context.Rooms.FindAsync(room.Id);

            if (dbRoom == null) return;

            dbRoom.No = room.No;
            dbRoom.NumberOfColumn = room.NumberOfColumn;
            dbRoom.NumberOfRow = room.NumberOfRow;
            dbRoom.Status = room.Status;
            
        }
        public async Task DeleteRoom(int id)
        {
            Room dbRoom = await _context.Rooms.FindAsync(id);

            if (dbRoom == null) return;

            dbRoom.Status = false;
        }

        public async Task<bool> CheckRoomExist(long managerId, int theaterId, int roomId)
        {
            bool isExist = false;
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT 1 " +
                    " FROM Theaters t JOIN Rooms r ON t.Id = r.TheaterId " +
                    " WHERE t.ManagerId = @managerId AND t.Id = @theaterId AND r.Id = @roomId ";

                object param = new { managerId, theaterId, roomId };
                int count = await con.ExecuteScalarAsync<int>(sql, param);

                if (count > 0) isExist = true;
            }
            return isExist;
        }

        public async Task<IEnumerable<RoomDto>> GetRoomByManagerIdAsync(long managerId)
        {
            IEnumerable<RoomDto> list = new List<RoomDto>();
            using (SqlConnection con = new SqlConnection(_connectionOption.FCinemaConnection))
            {
                string sql =
                    " SELECT r.Id " +
                    " FROM Theaters t JOIN Rooms r ON t.Id = r.TheaterId " +
                    " WHERE t.ManagerId = @managerId ";

                object param = new { managerId };

                list = await con.QueryAsync<RoomDto>(sql, param);

            }
            return list;
        }

        public async Task<IEnumerable<RoomNumberDTO>> GetRoomNumberFromTheater(int theaterid)
        {
            if (!await _context.Database.CanConnectAsync())
                return null;
            List<RoomNumberDTO> result = new List<RoomNumberDTO>();
            var query = _context.Rooms
                                .Where(a => a.TheaterId == theaterid)
                                .Select(a => new RoomNumberDTO
                                {
                                    RoomID = a.Id,
                                    RoomNumber = a.No
                                });
            if (!query.Any()) throw new Exception("EMPTYDATA");
            result = await query.ToListAsync();
            return result;
        }
    }
}
