namespace Term7MovieCore.Data.Request
{
    public class CompanyFilterRequest : ParentFilterRequest
    {
        public bool WithNoManager { set; get; } = false;

        public bool TheaterIncluded { set; get; } = true;
    }
}
