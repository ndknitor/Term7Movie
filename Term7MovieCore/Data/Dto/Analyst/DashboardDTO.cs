
namespace Term7MovieCore.Data.Dto.Analyst
{
    public class DashboardDTO
    {
        //FirstBoard
        public int TotalShowtimeQuantity { get; set; }
        public int OldShowtimeQuantity { get; set; } //tháng trước
        public int NewShowtimeQuantity { get; set; } //current
        public float PercentShowtimeChange { get; set; }
        //SecondBoard
        public int TotalTicketSoldQuantity { get; set; }
        public int OldTicketSoldQuantity { get; set; } //tháng trước
        public int NewTicketSoldQuantity { get; set; } //current
        public int PercentTicketSoldChange { get; set; }
        //ThirdBoard
        public decimal TotalIncomeQuantity { get; set; }
        public decimal OldIncomeQuantity { get; set; } //tháng trước
        public decimal NewIncomeQuantity { get; set; } //current
        public int PercentIncomeChange { get; set; }
    }
}
