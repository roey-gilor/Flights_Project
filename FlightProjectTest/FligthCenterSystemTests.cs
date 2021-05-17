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

            FlightCenterSystem.Instance.Login(out ILoginToken token, "admin", "9999");
            LoginToken<Administrator> loginToken = (LoginToken<Administrator>)token;
            LoggedInAdministratorFacade facade = (LoggedInAdministratorFacade)FlightCenterSystem.Instance.GetFacade(loginToken);

        }
    }
}
