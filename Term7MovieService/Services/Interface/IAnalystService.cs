using Term7MovieCore.Data.Response.Analyst;

namespace Term7MovieService.Services.Interface
{
    public interface IAnalystService
    {
        Task<DashboardResponse> GetQuickAnalystForDashboard(int companyid);
    }
}
