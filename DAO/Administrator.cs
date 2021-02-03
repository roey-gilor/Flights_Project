using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DAO
{
    public class Administrator : IPoco, IUser
    {
        public long Id { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public int Level { get; set; }
        public long User_Id { get; set; }
        public User User { get; set; }
        public Administrator()
        {

        }

        public Administrator(long id, string first_Name, string last_Name, int level, long user_Id, User user)
        {
            Id = id;
            First_Name = first_Name;
            Last_Name = last_Name;
            Level = level;
            User_Id = user_Id;
            User = user;
        }

        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as Administrator;
        }
        public static bool operator ==(Administrator a1, Administrator a2)
        {
            if (a1 is null && a2 is null)
                return true;
            if (a1 is null || a2 is null)
                return false;

            return a1.Id == a2.Id;
        }
        public static bool operator !=(Administrator a1, Administrator a2)
        {
            return !(a1 == a2);
        }
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
