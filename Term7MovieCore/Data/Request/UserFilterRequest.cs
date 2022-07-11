namespace Term7MovieCore.Data.Request
{
    public class UserFilterRequest : ParentFilterRequest
    {
        public string Email { set; get; }
        public bool IsManagerOnly { set; get; } = false;
        public bool IsCustomerOnly { set; get; } = false;
    }
}
