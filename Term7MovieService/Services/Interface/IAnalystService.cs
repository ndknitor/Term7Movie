using Term7MovieCore.Data.Response.Analyst;

namespace Term7MovieService.Services.Interface
{
    public interface IAnalystService
    {
        Task<DashboardResponse> GetQuickAnalystDashboardForManager(int companyid, long? managerid);
        Task<DashboardResponse> GetQuickAnalystDashboardForAdmin();
        Task<YearlyIncomeResponse> GetYearlyIncomeForManager(int companyid, int year);
        Task<YearlyIncomeResponse> GetYearlyIncomeForAdmin(int year);
    }
}
