﻿
namespace Term7MovieCore.Data.Dto
{
    public class TheaterDto
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string Latitude { set; get; } // North to South
        public string Longitude { set; get; } // West to East
        public int CompanyId { set; get; }
        public long ManagerId { set; get; }
        public bool Status { set; get; }
    }
}
