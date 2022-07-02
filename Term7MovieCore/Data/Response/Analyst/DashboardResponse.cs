using Term7MovieCore.Data.Dto.Analyst;

namespace Term7MovieCore.Data.Response.Analyst
{
    public class DashboardResponse : ParentResponse
    {
        public ShowtimeQuanityDTO ShowtimeDashboard { get; set; }
        public TicketSoldDTO TicketSoldDashboard { get; set; }
        public IncomeDTO IncomeDashboard { get; set; }
        public bool IsItStatistical { get; set; }
    }
}
