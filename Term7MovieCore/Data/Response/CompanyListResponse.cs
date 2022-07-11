using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class CompanyListResponse : ParentResponse
    {
        public PagingList<CompanyDto> Companies { set; get; }
    }
}
