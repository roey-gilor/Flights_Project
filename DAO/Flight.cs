using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class Flight : IPoco
    {
        public long Id { get; set; }
        public long Airline_Company_Id { get; set; }
        public long Origin_Country_Id { get; set; }
        public long Destination_Country_Id { get; set; }
        public DateTime Departure_Time { get; set; }
        public DateTime Landing_Time { get; set; }
        public int Remaining_Tickets { get; set; }
        public AirlineCompany Airline_Company { get; set; }
        public Flight()
        {

        }

        public Flight(long id, long airline_Company_Id, long origin_Country_Id, long destination_Country_Id, DateTime departure_Time,
            DateTime landing_Time, int remaining_Tickets, AirlineCompany airline_Company)
        {
            Id = id;
            Airline_Company_Id = airline_Company_Id;
            Origin_Country_Id = origin_Country_Id;
            Destination_Country_Id = destination_Country_Id;
            Departure_Time = departure_Time;
            Landing_Time = landing_Time;
            Remaining_Tickets = remaining_Tickets;
            Airline_Company = airline_Company;
        }
        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as Flight;
        }
        public static bool operator ==(Flight f1, Flight f2)
        {
            if (f1 is null && f2 is null)
                return true;
            if (f1 is null || f2 is null)
                return false;

            return f1.Id == f2.Id;
        }
        public static bool operator !=(Flight f1, Flight f2)
        {
            return !(f1 == f2);
        }
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
