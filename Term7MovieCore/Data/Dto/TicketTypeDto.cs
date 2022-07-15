namespace Term7MovieCore.Data.Dto
{
    public class TicketTypeDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int CompanyId { set; get; }
        public decimal DefaultPrice { set; get; }
    }
}
