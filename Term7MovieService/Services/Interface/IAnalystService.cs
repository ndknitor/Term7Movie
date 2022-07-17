using Term7MovieCore.Data.Response.Analyst;

namespace Term7MovieService.Services.Interface
{
    public interface IAnalystService
    {
        Task<DashboardResponse> GetDashboardManager(long? managerid, string time);
        Task<DashboardResponse> GetDashboardForAdmin(string time);
        Task<YearlyIncomeResponse> GetYearlyIncomeForManager(int year, long? managerid);
        Task<YearlyIncomeResponse> GetYearlyIncomeForAdmin(int year);
    }
}
