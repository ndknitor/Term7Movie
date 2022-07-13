namespace Term7MovieCore.Data.Dto
{
    public class TopUpHistoryDto
    {
        public Guid Id { get; set; }
        public long UserId { set; get; }
        public UserDTO User { get; set; }
        public DateTime RecordDate { set; get; }
        public decimal Amount { set; get; }
        public string Description { set; get; }
        public Guid? TransactionId { set; get; }
    }
}
