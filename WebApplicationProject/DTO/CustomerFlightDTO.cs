using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationProject.DTO
{
    public class CustomerFlightDTO
    {
        public long Id { get; set; }
        public string AirlineCompanyName { get; set; }
        public string OrigionCountry { get; set; }
        public string DestinationCountry { get; set; }
        public DateTime Departure_Time { get; set; }
        public DateTime Landing_Time { get; set; }
        public CustomerFlightDTO()
        {

        }
    }
}
