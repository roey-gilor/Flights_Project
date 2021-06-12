using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class Customer : IPoco, IUser
    {
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Address { get; set; }
        public string Phone_No { get; set; }
        public string Credit_Card_No { get; set; }
        public long User_Id { get; set; }
        public User User { get; set; }
        public string Password
        {
            get
            {
                return "";
            }
            set { }
        }
        public string Name
        {
            get
            {
                return User.User_Name;
            }
            set { }
        }


        public Customer()
        {

        }

        public Customer(long id, string first_Name, string last_Name, string address, string phone_No, string credit_Card_No, long user_Id, User user)
        {
            Id = id;
            First_Name = first_Name;
            Last_Name = last_Name;
            Address = address;
            Phone_No = phone_No;
            Credit_Card_No = credit_Card_No;
            User_Id = user_Id;
            User = user;
        }

        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as Customer;
        }
        public static bool operator ==(Customer c1, Customer c2)
        {
            if (c1 is null && c2 is null)
                return true;
            if (c1 is null || c2 is null)
                return false;

            return c1.Id == c2.Id;
        }
        public static bool operator !=(Customer c1, Customer c2)
        {
            return !(c1 == c2);
        }
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
