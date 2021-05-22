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
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken loginToken, "roey123", "12345");
            LoginToken<Administrator> token = (LoginToken<Administrator>)loginToken;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
        }
    }
}
