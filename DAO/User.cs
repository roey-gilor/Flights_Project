using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public class User : IPoco
    {
        public long Id { get; set; }
        public string User_Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public int User_Role { get; set; }
        public User()
        {

        }

        public User(long id, string user_Name, string password, string email, int user_Role)
        {
            Id = id;
            User_Name = user_Name;
            Password = password;
            Email = email;
            User_Role = user_Role;
        }
        public override int GetHashCode()
        {
            return (int)this.Id;
        }
        public override bool Equals(object obj)
        {
            return this == obj as User;
        }
        public static bool operator ==(User user1, User user2)
        {
            if (user1 is null && user2 is null)
                return true;
            if (user1 is null || user2 is null)
                return false;

            return user1.Id == user2.Id;
        }
        public static bool operator !=(User user1, User user2)
        {
            return !(user1 == user2);
        }
        public override string ToString()
        {
            return $"{JsonConvert.SerializeObject(this)}";
        }
    }
}
