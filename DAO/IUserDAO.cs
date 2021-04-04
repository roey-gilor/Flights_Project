using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface IUserDAO : IBasicDB<User>
    {
        User GetUserByUserName(string userName);
    }
}
