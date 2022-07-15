
namespace Term7MovieCore.Data.Dto.Analyst
{
    public class DashboardDTO
    {
        public ShowtimeQuanityDTO Showtime { get; set; }

        public TicketSoldDTO TicketSold { get; set; }

        public IncomeDTO Income { get; set; }

        //FirstBoard
        //public int TotalShowtimeQuantity { get; set; }
        //public int OldShowtimeQuantity { get; set; } //tháng trước
        //public int NewShowtimeQuantity { get; set; } //current
        //public float PercentShowtimeChange { get; set; }
        //public bool IsShowTimeUpOrDown { get; set; } // trưởng hợp ngang nhau thì cho là có lên
        //SecondBoard
        //public int TotalTicketSoldQuantity { get; set; }
        //public int OldTicketSoldQuantity { get; set; } //tháng trước
        //public int NewTicketSoldQuantity { get; set; } //current
        //public float PercentTicketSoldChange { get; set; }
        //public bool IsTicketSoldUpOrDown { get; set; } // trưởng hợp ngang nhau thì cho là có lên
        //ThirdBoard
        //public decimal TotalIncomeQuantity { get; set; }
        //public decimal OldIncomeQuantity { get; set; } //tháng trước
        //public decimal NewIncomeQuantity { get; set; } //current
        //public decimal PercentIncomeChange { get; set; }
        //public bool IsIncomeUpOrDown { get; set; } // trưởng hợp ngang nhau thì cho là có lên
    }

    public class TicketQuanityDTO
    {
        public int TotalTicketSoldQuantity { get; set; }
        public int OldTicketSoldQuantity { get; set; } //tháng trước
        public int NewTicketSoldQuantity { get; set; } //current
        public float PercentTicketSoldChange { get; set; }
        public bool IsTicketSoldUpOrDown { get; set; }
    }

    public class ShowtimeQuanityDTO
    {
        public int TotalShowtimeQuantity { get; set; }
        public int OldShowtimeQuantity { get; set; } //tháng trước
        public int NewShowtimeQuantity { get; set; } //current
        public float PercentShowtimeChange { get; set; }
        public bool IsShowtimeUpOrDown { get; set; }
    }

    public class TicketSoldDTO
    {
        public int TotalTicketSoldQuantity { get; set; }
        public int OldTicketSoldQuantity { get; set; } //tháng trước
        public int NewTicketSoldQuantity { get; set; } //current
        public float PercentTicketSoldChange { get; set; }
        public bool IsTicketSoldUpOrDown { get; set; }
    }

    public class IncomeDTO
    {
        public decimal TotalIncome { get; set; }
        public decimal OldIncome { get; set; } //tháng trước
        public decimal NewIncome { get; set; } //current
        public decimal PercentIncomeChange { get; set; }
        public bool IsIncomeUpOrDown { get; set; } // trưởng hợp ngang nhau thì cho là có lên
    }
}
