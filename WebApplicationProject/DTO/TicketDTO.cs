using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationProject.DTO
{
     public class TicketDTO
    {
        public long Id { get; set; }
        public long Flight_Id { get; set; }
        public string AirlineCompanyName { get; set; }
        public string OrigionCountry { get; set; }
        public string DestinationCountry { get; set; }
        public DateTime Departure_Time { get; set; }
        public DateTime Landing_Time { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Address { get; set; }
        public string Phone_No { get; set; }
        public TicketDTO()
        {

        }
    }
}
