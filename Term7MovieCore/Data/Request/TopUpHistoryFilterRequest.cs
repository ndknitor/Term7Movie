namespace Term7MovieCore.Data.Request
{
    public class TopUpHistoryFilterRequest : ParentFilterRequest
    {
        public string Email { set; get; }
        public long? UserId { set; get; }
        public bool IncludeUser { set; get; } = false;
    }
}
