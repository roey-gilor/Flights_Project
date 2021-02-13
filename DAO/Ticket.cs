using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class Ticket : IPoco
    {
        public long Id { get; set; }
        public long Flight_Id { get; set; }
        public long Customer_Id { get; set; }
        public Flight Flight { get; set; }
        public Customer Customer { get; set; }
        public Ticket()
        {

        }

        public Ticket(long id, long flight_Id, long customer_Id, Flight flight, Customer customer)
        {
            Id = id;
            Flight_Id = flight_Id;
            Customer_Id = customer_Id;
            Flight = flight;
            Customer = customer;
        }

        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as Ticket;
        }
        public static bool operator ==(Ticket t1, Ticket t2)
        {
            if (t1 is null && t2 is null)
                return true;
            if (t1 is null || t2 is null)
                return false;

            return t1.Id == t2.Id;
        }
        public static bool operator !=(Ticket t1, Ticket t2)
        {
            return !(t1 == t2);
        }
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
