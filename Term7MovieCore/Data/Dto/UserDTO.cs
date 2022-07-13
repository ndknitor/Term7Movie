namespace Term7MovieCore.Data.Dto
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PictureUrl { get; set; }
        public int? CompanyId { set; get; }
        public CompanyDto Company { set; get; }
        public decimal Point { get; set; }
        public int StatusId { set; get; }
        public string StatusName { set; get; }
        public RoleDto Role { set; get; }
    }
}
