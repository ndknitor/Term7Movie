using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class CompanyListResponse : ParentResponse
    {
        public IEnumerable<CompanyDto> Companies { set; get; }
    }
}
