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
    public class FligthCenterSystemTests
    {
        [TestMethod]
        public void CurrectLogginFacadeSuperAdmin_Test()
        {
            //FlightCenterSystem.Instance.Login(out ILoginToken token, "admin", "9999");
            //LoginToken<Administrator> loginToken = new LoginToken<Administrator>();
            //LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)FlightCenterSystem.Instance.GetFacade(loginToken);

            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "admin", "9999");
            LoginToken<Administrator> loginToken = (LoginToken<Administrator>)token;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)facadeBase;
            Assert.AreEqual(loginToken.User.Id, 0);

        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void WrongdLoginMainAdmin_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "admin", "9998");
            LoginToken<Administrator> loginToken = (LoginToken<Administrator>)token;
        }
        [TestMethod]
        public void TryLoginAdmin_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "danny121121", "fdsaa23");
            LoginToken<Administrator> loginToken = (LoginToken<Administrator>)token;
            Assert.AreEqual(loginToken.User.Id, 3);
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void WrongLoginAdmin_Test()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "danny121121", "265");
            LoginToken<Administrator> loginToken = (LoginToken<Administrator>)token;
        }
        [TestMethod]
        [ExpectedException(typeof(WrongCredentialsException))]
        public void UserNullException()
        {
            FlightCenterSystem.Instance.Login(out FacadeBase facadeBase, out ILoginToken token, "danny1211021", "fdsaa23");
        }
    }
}
