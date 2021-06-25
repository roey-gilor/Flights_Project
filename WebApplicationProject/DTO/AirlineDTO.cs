using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationProject.DTO
{
    public class AirlineDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country_Name { get; set; }
        public long User_Id { get; set; }
        public User User { get; set; }
    }
}
