using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class AirlineCompany : IPoco, IUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long Country_Id { get; set; }
        public long User_Id { get; set; }
        public User User { get; set; }
        public AirlineCompany()
        {

        }

        public AirlineCompany(long id, string name, long country_Id, long user_Id, User user)
        {
            Id = id;
            Name = name;
            Country_Id = country_Id;
            User_Id = user_Id;
            User = user;
        }
        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as AirlineCompany;
        }
        public static bool operator ==(AirlineCompany a1, AirlineCompany a2)
        {
            if (a1 is null && a2 is null)
                return true;
            if (a1 is null || a2 is null)
                return false;

            return a1.Id == a2.Id;
        }
        public static bool operator !=(AirlineCompany a1, AirlineCompany a2)
        {
            return !(a1 == a2);
        }
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
