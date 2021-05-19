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
    public class AirlineCompanyFacadeTests
    {
        static IAirlineDAO _airlineDAO = new AirlineDAOPGSQL();
        static IFlightDAO _flightDAO = new FlightDAOPGSQL();
        [TestMethod]
        public void GetAllFlights_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            IList<Flight> list = facade.GetAllFlights(loginToken);
            Assert.AreEqual(list.Count, 3);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesToGetFlightsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            IList<Flight> list = facade.GetAllFlights(null);
        }
        [TestMethod]
        public void GetAllTickets_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "dana432", "gdfds");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            IList<Ticket> list = facade.GetAllTickets(loginToken);
            Assert.AreEqual(list.Count, 4);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesToGetTicketsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "dana432", "gdfds");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            IList<Ticket> list = facade.GetAllTickets(null);
        }
        [TestMethod]
        public void MofidyAirlineDetails_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase; 
            AirlineCompany airline = new AirlineCompany
            {
                Id = loginToken.User.Id,
                Name = loginToken.User.Name,
                Country_Id = 4,
                User_Id = loginToken.User.User_Id
            };
            facade.MofidyAirlineDetails(loginToken, airline);
            Assert.AreEqual(loginToken.User.Country_Id, 4);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesToModifyDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            AirlineCompany airline = new AirlineCompany
            {
                Id = loginToken.User.Id,
                Name = loginToken.User.Name,
                Country_Id = 4,
                User_Id = loginToken.User.User_Id
            };
            facade.MofidyAirlineDetails(null, airline);
        }
        [TestMethod]
        public void UpdateFlight_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            Flight flight = _flightDAO.Get(6);
            DateTime dateTime= new DateTime(2021, 1, 13, 8, 00, 00);
            flight.Departure_Time = dateTime;
            facade.UpdateFlight(loginToken, flight);
            flight = _flightDAO.Get(6);
            Assert.AreEqual(flight.Departure_Time, dateTime);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesToUpdateFlightException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            Flight flight = _flightDAO.Get(6);
            DateTime dateTime = new DateTime(2021, 1, 13, 8, 00, 00);
            flight.Departure_Time = dateTime;
            facade.UpdateFlight(null, flight);
        }
        [TestMethod]
        public void CreateFlight_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "dana432", "gdfds");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            Flight flight = new Flight
            {
                Airline_Company_Id = 1,
                Origin_Country_Id = 4,
                Destination_Country_Id = 3,
                Departure_Time = new DateTime(2021, 5, 14, 19, 30, 00),
                Landing_Time = new DateTime(2021, 5, 14, 22, 30, 00),
                Remaining_Tickets = 30
            };
            facade.CreateFlight(loginToken, flight);
            Flight check_flight = _flightDAO.Get(10);
            Assert.AreEqual(check_flight.Id, 10);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesToCreateFlightException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "dana432", "gdfds");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            facade.CreateFlight(null, new Flight());
        }
        [TestMethod]
        public void ChangeMyPassword_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "54321");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            facade.ChangeMyPassword(loginToken, "54321", "12345");
            Assert.AreEqual(loginToken.User.User.Password, "12345");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void OldPasswordDoesntMatchToTheSystem()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "12345");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            facade.ChangeMyPassword(loginToken, loginToken.User.User.Password, "12345");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void NewPasswordEqualsToTheOldOneException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "12345");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            facade.ChangeMyPassword(loginToken, "12345", "12345");
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesTochangePasswordException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "adi213", "12345");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            facade.ChangeMyPassword(null, "12345", "111");
        }
        [TestMethod]
        public void CancelFlight_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "dana432", "gdfds");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            Flight flight = _flightDAO.Get(1);
            facade.CancelFlight(loginToken, flight);
            IList<Flight> list = _flightDAO.GetAll();
            Assert.AreEqual(list.Count, 5);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAirlineException))]
        public void NullUserTriesCancelFlightException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadebase, out ILoginToken token, "dana432", "gdfds");
            LoginToken<AirlineCompany> loginToken = (LoginToken<AirlineCompany>)token;
            LoggedInAirlineFacade facade = (LoggedInAirlineFacade)facadebase;
            facade.CancelFlight(null, new Flight());
        }
    }
}
