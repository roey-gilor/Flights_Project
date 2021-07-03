using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoginService : FacadeBase, ILoginService
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        IAirlineDAO _airlineDAO = new AirlineDAOPGSQL();
        ICustomerDAO _customerDAO = new CustomerDAOPGSQL();
        IAdminDAO _adminDAO = new AdminDAOPGSQL();
        IUserDAO _userDAO = new UserDAOPGSQL();
        internal static Administrator mainAdmin = new Administrator()
        {
            Id = 0,
            First_Name = "Main",
            Last_Name = "admin",
            Level = 3,
            User_Id = 0,
            User = new User()
            {
                Id = 0,
                User_Name = "admin",
                Password = "99999",
                User_Role = 1
            }
        };

        public bool TryLogin(out FacadeBase facade, out ILoginToken loginToken, string userName, string password)
        {
            if (userName == "admin")
            {
                if (password == "99999")
                {
                    facade = new LoggedInAdministratorFacade();
                    loginToken = new LoginToken<Administrator>()
                    {
                        User = mainAdmin
                    };
                    log.Info("Main Administrator has logged in to the system");
                    return true;
                }
                else
                {
                    log.Error("One or more of the super admin details are wrong");
                    throw new WrongCredentialsException("One or more of the super admin details are wrong");
                }
            }
            User user;
            try
            {
                user = _userDAO.GetUserByUserName(userName);
            }
            catch (Exception ex)
            {
                log.Error($"Could not find user: {ex.Message}");
                throw new WrongCredentialsException($"One or more of the details are wrong: {ex.Message}");
            }
            if (user.Password == password)
            {
                switch (user.User_Role)
                {
                    case 1:
                        {
                            facade = new LoggedInAdministratorFacade();
                            Administrator admin = _adminDAO.GetAdminByUserId(user.Id);
                            loginToken = new LoginToken<Administrator>()
                            {
                                User = admin
                            };
                            break;
                        }
                    case 2:
                        {
                            facade = new LoggedInAirlineFacade();
                            AirlineCompany airlineCompany = _airlineDAO.GetAirlineByUserId(user.Id);
                            loginToken = new LoginToken<AirlineCompany>()
                            {
                                User = airlineCompany
                            };
                            break;
                        }
                    case 3:
                        {
                            facade = new LoggedInCustomerFacade();
                            Customer customer = _customerDAO.GetCustomerByUserId(user.Id);
                            loginToken = new LoginToken<Customer>()
                            {
                                User = customer
                            };
                            break;
                        }
                    default:
                        {
                            facade = new AnonymousUserFacade();
                            loginToken = null;
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
    }
}
