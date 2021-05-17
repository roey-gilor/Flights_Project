﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        [TestMethod]
        public void GetAllMyFlights_Test()
        {
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);

            IList<Flight> list= facade.GetAllMyFlights(loginToken);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void GetAllMyFlightsSendNullGetException()
        {
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);

            IList<Flight> list = facade.GetAllMyFlights(null);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        public void PurchaseTicket_Test()
        {
            Flight flight = _flightDAO.Get(9);
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.PurchaseTicket(loginToken, flight);
            Assert.AreEqual(flight.Remaining_Tickets, 44);
        }
        [TestMethod]
        [ExpectedException(typeof(CustomerAlreadyBoughtTicketException))]
        public void CustomerBoughtTicketToTheSameFlightTwiceException()
        {
            Flight flight = _flightDAO.Get(1);
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.PurchaseTicket(loginToken, flight);
        }
        [TestMethod]
        [ExpectedException(typeof(OutOfTicketsException))]
        public void OutOfTicketsException_Test()
        {
            Flight flight = _flightDAO.Get(7);
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.PurchaseTicket(loginToken, flight);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void NullObjectTryToBuyTicket()
        {
            Flight flight = _flightDAO.Get(9);
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.PurchaseTicket(null, flight);
        }
        [TestMethod]
        public void CancelTicket_Test()
        {
            Ticket ticket = _ticketDAO.Get(15);
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.CancelTicket(loginToken, ticket);
            Flight flight = _flightDAO.Get(9);
            Assert.AreEqual(flight.Remaining_Tickets, 45);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void CancelTicketNullRefException()
        {
            Ticket ticket = _ticketDAO.Get(1);
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "vzd474");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.CancelTicket(null, ticket);
        }
        [TestMethod]
        public void ChangeMyPassword_Test()
        {
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "12234");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.ChangeMyPassword(loginToken, "12234", "ddvrew1");
            Assert.AreEqual(loginToken.User.User.Password, "ddvrew1");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void OldPasswordDoesntMatchToTheSystem()
        {
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.ChangeMyPassword(loginToken, "vzd474", "12234");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void NewPasswordEqualsToTheOldOneException()
        {
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.ChangeMyPassword(loginToken, "ddvrew1", "ddvrew1");
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByCustomerException))]
        public void NullTriesToChangePasswordException()
        {
            FlightCenterSystem.Instance.Login(out ILoginToken token, "uri321", "ddvrew1");
            LoginToken<Customer> loginToken = (LoginToken<Customer>)token;
            LoggedInCustomerFacade facade = (LoggedInCustomerFacade)FlightCenterSystem.Instance.GetFacade(loginToken);
            facade.ChangeMyPassword(null, "ddvrew1", "111");
        }
    }
}
