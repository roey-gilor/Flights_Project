using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface IUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }

    }
}
