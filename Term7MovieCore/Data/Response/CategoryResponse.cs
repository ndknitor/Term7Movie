using Term7MovieCore.Data.Dto;

namespace Term7MovieCore.Data.Response
{
    public class CategoryResponse : ParentResponse
    {
        public IEnumerable<CategoryDTO> Categories { get; set; }
    }
}
