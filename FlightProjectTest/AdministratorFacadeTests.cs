using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using BusinessLogic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlightProjectTest
{
    [TestClass]
    public class AdministratorFacadeTests
    {
        IUserDAO _userDAO = new UserDAOPGSQL();
        IAdminDAO _adminDAO = new AdminDAOPGSQL();
        ICountryDAO _countryDAO = new CountryDAOPGSQL();
        IAirlineDAO _airlineDAO = new AirlineDAOPGSQL();
        ICustomerDAO _customerDAO = new CustomerDAOPGSQL();

        [TestMethod]
        public void GetAllCustomers_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            IList<Customer> list = facade.GetAllCustomers(token);
            Assert.AreEqual(list.Count, 2);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToGetAllCustomer()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            IList<Customer> list = facade.GetAllCustomers(null);
        }
        [TestMethod]
        public void CreateAdmin_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;

            User user = new User
            {
                User_Name = "Bar_36",
                Password = "rvdsgr",
                Email = "bar12121213@gmail.com",
                User_Role = 1
            };
            _userDAO.Add(user);
            Administrator administrator = new Administrator
            {
                First_Name = "Bar",
                Last_Name = "Cohen",
                Level = 2,
                User_Id = 16,
                User = user
            };
            facade.CreateAdmin(token, administrator);
            Assert.AreEqual(_adminDAO.GetAll().Count, 4);
        }
        [TestMethod]
        public void MainAdminAddLevelThreeAdmin()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "admin", "9999");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;

            User user = new User
            {
                User_Name = "Shlomi5431",
                Password = "bdnjr8",
                Email = "shlomi98@gmail.com",
                User_Role = 1
            };
            _userDAO.Add(user);
            Administrator administrator = new Administrator
            {
                First_Name = "Shlomi",
                Last_Name = "Banai",
                Level = 3,
                User_Id = 20,
                User = user
            };
            facade.CreateAdmin(token, administrator);
            Assert.AreEqual(_adminDAO.GetAll().Count, 4);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void HighesAdminTriesToAddHighestAdminException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;

            Administrator administrator = new Administrator
            {
                First_Name = "Shlomi",
                Last_Name = "Banai",
                Level = 3,
                User_Id = 17,
                User = new User()
            };
            facade.CreateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void NotLevelThreeAdminTriesToAddNewAdminException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;

            Administrator administrator = new Administrator
            {
                First_Name = "Shlomi",
                Last_Name = "Banai",
                Level = 3,
                User_Id = 17,
                User = new User()
            };
            facade.CreateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToCreateNewAdmin()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = new Administrator
            {
                First_Name = "Shlomi",
                Last_Name = "Banai",
                Level = 3,
                User_Id = 17,
                User = new User()
            };
            facade.CreateAdmin(null, administrator);
        }
        [TestMethod]
        public void CreateCountry_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = new Country()
            {
                Name = "Greek"
            };
            facade.CreateCountry(token, country);
            Assert.AreEqual(_countryDAO.GetAll().Count, 5);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void DuplicateCountryException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = new Country()
            {
                Name = "Greek"
            };
            facade.CreateCountry(token, country);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void NotLevelThreeUserTriesToAddNewCountry()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = new Country()
            {
                Name = "Greek"
            };
            facade.CreateCountry(token, country);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToAddNewCountryException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.CreateCountry(null, new Country());
        }
        [TestMethod]
        public void CreateNewAirline_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "amir54", "fsdf3");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            User user = new User()
            {
                User_Name = "Wings",
                Password = "vcregs3",
                Email = "wings@gmail.com",
                User_Role = 2
            };
            _userDAO.Add(user);
            AirlineCompany airlineCompany = new AirlineCompany
            {
                Name = "flying wings",
                Country_Id = 2,
                User_Id = 18,
                User = user
            };
            facade.CreateNewAirline(token, airlineCompany);
            Assert.AreEqual(_airlineDAO.GetAll().Count, 3);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void DuplicateAirlineException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "amir54", "fsdf3");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            User user = new User()
            {
                User_Name = "Wings",
                Password = "vcregs3",
                Email = "wings@gmail.com",
                User_Role = 2
            };
            AirlineCompany airlineCompany = new AirlineCompany
            {
                Name = "flying wings",
                Country_Id = 2,
                User_Id = 18,
                User = user
            };
            facade.CreateNewAirline(token, airlineCompany);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowestAdminTriesToAddNewAirlineException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            User user = new User()
            {
                User_Name = "Wings",
                Password = "vcregs3",
                Email = "wings@gmail.com",
                User_Role = 2
            };
            AirlineCompany airlineCompany = new AirlineCompany
            {
                Name = "flying wings",
                Country_Id = 2,
                User_Id = 18,
                User = user
            };
            facade.CreateNewAirline(token, airlineCompany);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToAddNewAirline()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "amir54", "fsdf3");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.CreateNewAirline(null, new AirlineCompany());
        }
        [TestMethod]
        public void CreateNewCustomer_Tets()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "amir54", "fsdf3");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            User user = new User()
            {
                User_Name = "Malka4444",
                Password = "bgfvs",
                Email = "Malka@walla.com",
                User_Role = 3
            };
            _userDAO.Add(user);
            Customer customer = new Customer
            {
                First_Name = "Malka",
                Last_Name = "Tzion",
                Address = "Halevi 35",
                Phone_No = "050-913768",
                Credit_Card_No = "254565414",
                User_Id = 19,
                User = user
            };
            facade.CreateNewCustomer(token, customer);
            Assert.AreEqual(_customerDAO.GetAll().Count, 3);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void DuplicateCustomerDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "amir54", "fsdf3");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            User user = new User()
            {
                User_Name = "Malka4444",
                Password = "bgfvs",
                Email = "Malka@walla.com",
                User_Role = 3
            };
            Customer customer = new Customer
            {
                First_Name = "Malka",
                Last_Name = "Tzion",
                Address = "Halevi 35",
                Phone_No = "050-913768",
                Credit_Card_No = "757222821",
                User_Id = 19,
                User = user
            };
            facade.CreateNewCustomer(token, customer);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowestLevellAdminTriesToCreateNewCustomerException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.CreateNewCustomer(token, new Customer());
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToAddNewCustomerException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "amir54", "fsdf3");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.CreateNewCustomer(null, new Customer());
        }
        [TestMethod]
        public void RemoveAdmin_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(2);
            facade.RemoveAdmin(token, administrator);
            Assert.AreEqual(_adminDAO.GetAll().Count, 4);
        }
        [TestMethod]
        public void MainAdminRemovesLevelThreeAdmin()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "admin", "9999");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(6);
            facade.RemoveAdmin(token, administrator);
            Assert.AreEqual(_adminDAO.GetAll().Count, 3);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LevelThreeAdminTriesToRemoveLevelThreeAdminException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(7);
            facade.RemoveAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void NotLevelThreeAdminTriesToRemoveAdminException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveAdmin(token, new Administrator());
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToRemoveAdminException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveAdmin(null, new Administrator());
        }
        [TestMethod]
        public void RemoveCustomer_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Customer customer = _customerDAO.Get(2);
            facade.RemoveCustomer(token, customer);
            Assert.AreEqual(_customerDAO.GetAll().Count, 2);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowestAdminTriesToRemoveCustomerException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveCustomer(token, new Customer());
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullAdminTriesToRemoveCustomer()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveCustomer(null, new Customer());
        }
        [TestMethod]
        public void RemoveAirline_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            AirlineCompany airlineCompany = _airlineDAO.Get(1);
            facade.RemoveAirline(token, airlineCompany);
            Assert.AreEqual(_airlineDAO.GetAll().Count, 2);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowestAdminTriesToRemoveAirlineException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveAirline(token, new AirlineCompany());
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToRemoveAirline()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveAirline(null, new AirlineCompany());
        }
        [TestMethod]
        public void RemoveCountry_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = _countryDAO.Get(3);
            facade.RemoveCountry(token, country);
            Assert.AreEqual(_countryDAO.GetAll().Count, 4);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowerAdminTriesToRemoveCountry()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveCountry(token, new Country());
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToRemoveCountry()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.RemoveCountry(null, new Country());
        }
        [TestMethod]
        public void UpdateAdmin_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(5);
            administrator.Last_Name = "Valdman";
            facade.UpdateAdmin(token, administrator);
            Assert.AreEqual(_adminDAO.Get(5).Last_Name, "Valdman");
        }
        [TestMethod]
        public void AdminUpdatedHisOwnDetails()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = token.User;
            administrator.User.Email = "TheBar@gmail.com";
            facade.UpdateAdmin(token, administrator);
            Assert.AreEqual(token.User.User.Email, "TheBar@gmail.com");
        }
        [TestMethod]
        public void MainAdminUpdatedLevelThreeUser()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "admin", "9999");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(1);
            administrator.Last_Name = "Levy";
            facade.UpdateAdmin(token, administrator);
            Assert.AreEqual(_adminDAO.Get(1).Last_Name, "Levy");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void SelfUpdateDuplicateDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = token.User;
            administrator.User.User_Name = "danny121121";
            facade.UpdateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void UpdateAdminDuplicateDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(1);
            administrator.User.Email = "TheBar@gmail.com";
            facade.UpdateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void MainAdminUpdateDuplicteDetailsToLevelThreeUserException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "admin", "9999");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(7);
            administrator.User.Email = "TheBar@gmail.com";
            facade.UpdateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowerAdminTriesToUpdateAdmin()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(1);
            administrator.User.User_Name = "1111";
            facade.UpdateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowerAdminTriesToUpdateLevelThreeAdmin()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Administrator administrator = _adminDAO.Get(7);
            administrator.User.User_Name = "1111";
            facade.UpdateAdmin(token, administrator);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToUpdateAdmin()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.UpdateAdmin(null, new Administrator());
        }
        [TestMethod]
        public void UpdateAirlineDetails_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            AirlineCompany airlineCompany = _airlineDAO.Get(7);
            airlineCompany.Name = "flying_wings";
            facade.UpdateAirlineDetails(token, airlineCompany);
            Assert.AreEqual(_airlineDAO.Get(7).Name, "flying_wings");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void DuplicateAirlineDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            AirlineCompany airlineCompany = _airlineDAO.Get(7);
            User user = airlineCompany.User;
            user.User_Name = "roey123";
            facade.UpdateAirlineDetails(token, airlineCompany);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToUpdateAirlineDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.UpdateAirlineDetails(null, new AirlineCompany());
        }
        [TestMethod]
        public void UpdateCountry_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = _countryDAO.Get(2);
            country.Name = "USA";
            facade.UpdateCountry(token, country);
            Assert.AreEqual(_countryDAO.Get(2).Name, "USA");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void DuplicateCountryNameException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = _countryDAO.Get(2);
            country.Name = "Italy";
            facade.UpdateCountry(token, country);
        }
        [TestMethod]
        [ExpectedException(typeof(AdministratorDoesntHaveSanctionException))]
        public void LowerLevelAdmiinTriesToUpdateCountryException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "Bar_36", "rvdsgr");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Country country = _countryDAO.Get(2);
            country.Name = "lalala";
            facade.UpdateCountry(token, country);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToUpdateCountryException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "danny121121", "fdsaa23");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.UpdateCountry(null, new Country());
        }
        [TestMethod]
        public void UpdateCustomerDetails_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Customer customer = _customerDAO.Get(1);
            customer.User.Email = "UriBuri123@gmail.com";
            facade.UpdateCustomerDetails(token, customer);
            Assert.AreEqual(_customerDAO.Get(1).User.Email, "UriBuri123@gmail.com");
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void DuplicateCustomerDetailsWhenUpdateException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Customer customer = _customerDAO.Get(5);
            customer.Credit_Card_No = "1287";
            facade.UpdateCustomerDetails(token, customer);
        }
        [TestMethod]
        [ExpectedException(typeof(WasntActivatedByAdministratorException))]
        public void NullUserTriesToUpdateCustomerDetailsException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            facade.UpdateCustomerDetails(null, new Customer());
        }
    }
}
