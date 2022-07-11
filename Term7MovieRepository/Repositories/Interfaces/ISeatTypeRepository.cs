using Term7MovieCore.Data.Dto;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ISeatTypeRepository
    {
        Task<IEnumerable<SeatTypeDto>> GetAllSeatTypeAsync();

        Task<SeatTypeDto> GetSeatTypeByIdAsync(int id);

        Task UpdateSeatTypeAsync(SeatType seatType);
    }
}
