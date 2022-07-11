using Term7MovieCore.Data.Response.Analyst;

namespace Term7MovieService.Services.Interface
{
    public interface IAnalystService
    {
        Task<DashboardResponse> GetQuickAnalystDashboardForManager(int companyid, long? managerid);
        Task<DashboardResponse> GetQuickAnalystDashboardForAdmin();
    }
}
