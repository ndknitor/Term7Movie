using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITheaterRepository
    {
        Task<PagingList<TheaterDto>> GetAllTheaterAsync(TheaterFilterRequest request);
        Task<TheaterDto> GetTheaterByIdAsync(int id);
        Task CreateTheaterAsync(Theater theater);
        Task UpdateTheaterAsync(Theater theater);
        Task DeleteTheaterAsync(int id);
    }
}
