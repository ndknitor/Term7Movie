using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;
using Term7MovieCore.Data.Request;
using Term7MovieCore.Entities;

namespace Term7MovieRepository.Repositories.Interfaces
{
    public interface ITopUpHistoryRepository
    {
        Task<PagingList<TopUpHistoryDto>> GetAllAsync(TopUpHistoryFilterRequest request);
        Task CreateTopUpHistory(TopUpHistory topUpHistory);
    }
}
