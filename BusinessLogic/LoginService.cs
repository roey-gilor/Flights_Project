using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    class LoginService : ILoginService
    {
        IAirlineDAO _airlineDAO;
        ICustomerDAO _customerDAO;
        IAdminDAO _adminDAO;
        IUserDAO _userDAO;

        public bool TryLogin(out ILoginToken loginToken, string userName, string password)
        {
            if (userName == "admin")
            {
                if (password == "9999")
                {
                    loginToken = new LoginToken<Administrator>();
                    return true;
                }
                else
                    throw new WrongCredentialsException("One or more of the details are wrong");
            }
            User user = _userDAO.GetUserByUserName(userName);
            if (user != null)
            {
                if (user.Password == password)
                {
                    loginToken = new LoginToken<Administrator>();
                    return true;
                }
            }
            loginToken = null;
            return true;
        }
    }
}
