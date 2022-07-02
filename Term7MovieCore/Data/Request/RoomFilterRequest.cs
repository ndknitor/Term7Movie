using System.ComponentModel.DataAnnotations;

namespace Term7MovieCore.Data.Request
{
    public class RoomFilterRequest : ParentFilterRequest
    {
        [Required]
        public int TheaterId { set; get; }
    }
}
