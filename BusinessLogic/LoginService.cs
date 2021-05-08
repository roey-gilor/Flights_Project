using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoginService : ILoginService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
                    log.Info("Super Administrator has logged in to the system");
                    return true;
                }
                else
                {
                    log.Error("One or more of the super admin details are wrong");
                    throw new WrongCredentialsException("One or more of the super admin details are wrong");
                }
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
                    log.Info($"User {userName} has logged in to the system");
                    return true;
                }
                else
                {
                    log.Error($"One or more of the user {userName} details are wrong");
                    throw new WrongCredentialsException($"One or more of the user {userName} details are wrong");
                }
            }
            else
            {
                log.Error("An unknown user tried to Enter the system");
                throw new WasntActivatedByUserException("An unknown user tried to Enter the system");
            }
        }
    }
}
