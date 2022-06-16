using Term7MovieCore.Data.Collections;
using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class UserListResponse : ParentResponse
    {
        public PagingList<UserDTO> Users { set; get; }
    }
}
