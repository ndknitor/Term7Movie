using Term7MovieCore.Data.Dto.Analyst;

namespace Term7MovieCore.Data.Response.Analyst
{
    public class DashboardResponse : ParentResponse
    {
        public DashboardDTO DashBoardInfo { get; set; }
        public bool IsItStatistical { get; set; }
    }
}
