using Term7MovieCore.Data.Request;
using Term7MovieCore.Data.Response;

namespace Term7MovieService.Services.Interface
{
    public interface ITopUpService
    {
        Task<ParentResponse> TopUpAsync(TopUpRequest request, long userId);
        Task<ParentResultResponse> GetAllTopUpHistoryAsync(TopUpHistoryFilterRequest request, long userId, string role);
    }
}
