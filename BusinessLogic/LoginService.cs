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

        public bool TryLogin(out FacadeBase facade, out ILoginToken loginToken, string userName, string password)
        {
            if (userName == "admin")
            {
                if (password == "9999")
                {
                    loginToken = new LoginToken<Administrator>();
                    facade = new LoggedInAdministratorFacade();
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
                    switch (user.User_Role)
                    {
                        case 1:
                            {
                                loginToken = new LoginToken<Administrator>();
                                facade = new LoggedInAdministratorFacade();
                                break;
                            }
                        case 2:
                            {
                                loginToken = new LoginToken<AirlineCompany>();
                                facade = new LoggedInAirlineFacade();
                                break;
                            }
                        default:
                            {
                                loginToken = new LoginToken<Customer>();
                                facade = new LoggedInCustomerFacade();
                                break;
                            }
                    }
                    return true;
                }
                else
                    throw new WrongCredentialsException("One or more of the details are wrong");
            }
            else
                throw new WrongCredentialsException("One or more of the details are wrong");
        }
    }
}
