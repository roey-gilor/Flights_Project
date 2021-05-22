using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using DAO;

namespace FlightProjectTest
{
    [TestClass]
    public class CustomerFacadeTests
    {
        static IFlightDAO _flightDAO = new FlightDAOPGSQL();
        static ITicketDAO _ticketDAO = new TicketDAOPGSQL();
        static IUserDAO _userDAO = new UserDAOPGSQL();
        [TestMethod]
        public void GetAllMyFlights_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;

            IList<Flight> list= facade.GetAllMyFlights(loginToken);
            Assert.AreEqual(list.Count, 1);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void GetAllMyFlightsSendNullGetException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;

            IList<Flight> list = facade.GetAllMyFlights(null);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        public void PurchaseTicket_Test()
        {
            Flight flight = _flightDAO.Get(9);
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.PurchaseTicket(loginToken, flight);
            Assert.AreEqual(flight.Remaining_Tickets, 44);
        }
        [TestMethod]
        [ExpectedException(typeof(CustomerAlreadyBoughtTicketException))]
        public void CustomerBoughtTicketToTheSameFlightTwiceException()
        {
            Flight flight = _flightDAO.Get(1);
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.PurchaseTicket(loginToken, flight);
        }
        [TestMethod]
        [ExpectedException(typeof(OutOfTicketsException))]
        public void OutOfTicketsException_Test()
        {
            Flight flight = _flightDAO.Get(7);
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.PurchaseTicket(loginToken, flight);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void NullObjectTryToBuyTicket()
        {
            Flight flight = _flightDAO.Get(9);
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.PurchaseTicket(null, flight);
        }
        [TestMethod]
        public void CancelTicket_Test()
        {
            Ticket ticket = _ticketDAO.Get(15);
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.CancelTicket(loginToken, ticket);
            Flight flight = _flightDAO.Get(9);
            Assert.AreEqual(flight.Remaining_Tickets, 45);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void CancelTicketNullRefException()
        {
            Ticket ticket = _ticketDAO.Get(1);
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.CancelTicket(null, ticket);
        }
        [TestMethod]
        public void ChangeMyPassword_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "12234");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.ChangeMyPassword(loginToken, "12234", "ddvrew1");
            Assert.AreEqual(loginToken.User.User.Password, "ddvrew1");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void OldPasswordDoesntMatchToTheSystem()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.ChangeMyPassword(loginToken, "vzd474", "12234");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void NewPasswordEqualsToTheOldOneException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.ChangeMyPassword(loginToken, "ddvrew1", "ddvrew1");
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void NullTriesToChangePasswordException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.ChangeMyPassword(null, "ddvrew1", "111");
        }
        [TestMethod]
        public void UpdateUserDetails_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "shany805", "dsaasa");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            User user = new User
            {
                Id = loginToken.User.User.Id,
                User_Name = "shany203",
                Password = loginToken.User.User.Password,
                Email = loginToken.User.User.Email,
                User_Role = loginToken.User.User.User_Role
            };
            facade.UpdateUserDetails(loginToken, user);
            Assert.AreEqual(_userDAO.Get(6).User_Name, "shany203");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void WrongDetailsToUpdateUserException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "shany203", "dsaasa");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            User user = new User
            {
                Id = loginToken.User.User.Id,
                User_Name = loginToken.User.User.User_Name,
                Password = loginToken.User.User.Password,
                Email = "danny@gmail.com",
                User_Role = loginToken.User.User.User_Role
            };
            facade.UpdateUserDetails(loginToken, user);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void NullUserTriesToUpdateDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "shany203", "dsaasa");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)facadeBase;
            facade.UpdateUserDetails(null, new User());
        }
    }
}
