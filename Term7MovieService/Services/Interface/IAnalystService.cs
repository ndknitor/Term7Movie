using Term7MovieCore.Data.Response.Analyst;

namespace Term7MovieService.Services.Interface
{
    public interface IAnalystService
    {
        Task<DashboardResponse> GetQuickAnalystDashboardForManager(long? managerid);
        Task<DashboardResponse> GetQuickAnalystDashboardForAdmin();
        Task<YearlyIncomeResponse> GetYearlyIncomeForManager(int year, long? managerid);
        Task<YearlyIncomeResponse> GetYearlyIncomeForAdmin(int year);
    }
}
