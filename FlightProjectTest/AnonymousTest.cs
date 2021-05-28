using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using BusinessLogic;
using DAO;

namespace FlightProjectTest
{
    [TestClass]
    public class AnonymousTest
    {
        ICustomerDAO _customerDAO = new CustomerDAOPGSQL();
        [TestMethod]
        public void GetAllAirlineCompanies_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list= anonymous.GetAllAirlineCompanies();
            Assert.IsTrue(list.Count > 0);
        }
        [TestMethod]
        public void GetAllFlights_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list = anonymous.GetAllFlights();
            Assert.IsTrue(list.Count > 0);
        }
        [TestMethod]
        public void GetAllFlightsVacancy_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list = anonymous.GetAllFlightsVacancy();
            Assert.IsTrue(list.Count > 0);
        }
        [TestMethod]
        public void GetFlightsByDepatrureDate_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list = anonymous.GetFlightsByDepatrureDate(new System.DateTime(2021, 01, 01));
            Assert.AreEqual(list.Count, 1);
        }
        [DataTestMethod]
        public void GetFlightsByDestinationCountry_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list = anonymous.GetFlightsByDestinationCountry(3);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        public void GetFlightsByLandingDate_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list = anonymous.GetFlightsByLandingDate(new System.DateTime(2021, 01, 24));
            Assert.AreEqual(list.Count, 1);
        }
        [TestMethod]
        public void GetFlightsByOriginCountry_Test()
        {
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            var list = anonymous.GetFlightsByOriginCountry(2);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        public void AddNewUser_Test()
        {
            IUserDAO _userDAO = new UserDAOPGSQL();
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            User user = new User
            {
                User_Name = "Moshe817",
                Password = "rerert",
                Email = "moshe111@gmail.com",
                User_Role = 3
            };
            Customer customer = new Customer
            {
                First_Name = "Moshe",
                Last_Name = "yakov",
                Address = "king5",
                Phone_No = "050-1351151",
                Credit_Card_No = "1212123545",
                User = user
            };
            anonymous.AddNewCustomer(customer);
            Assert.AreEqual(_customerDAO.GetAll().Count, 3);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void WrongDetailsToNewUserException()
        {
            IUserDAO _userDAO = new UserDAOPGSQL();
            AnonymousUserFacade anonymous = new AnonymousUserFacade();
            User user = new User
            {
                User_Name = "Moshe87",
                Password = "rerert",
                Email = "moshe@gmail.com",
                User_Role = 3
            };
            Customer customer = new Customer();
            customer.User = user;
            anonymous.AddNewCustomer(customer);
        }
    }
}
