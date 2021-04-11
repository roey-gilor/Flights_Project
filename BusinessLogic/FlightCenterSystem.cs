using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class FlightCenterSystem
    {
        private static FlightCenterSystem m_Instance;
        static object key = new object();
        static LoginService loginService = new LoginService();
        public static FlightCenterSystem Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    lock (key)
                    {
                        if (m_Instance == null)
                            m_Instance = new FlightCenterSystem();
                    }
                }
                return m_Instance;
            }
        }
        private FlightCenterSystem()
        {
            Thread thread;
            
        }
        public FacadeBase GetFacade(out ILoginToken loginToken, string username, string password)
        {
            try
            {
                FacadeBase facade;
                bool res = loginService.TryLogin(out facade, out loginToken, username, password);
                return facade;
            }
            catch (WrongCredentialsException ex)
            {
                throw;
            }
        }
        private TimeSpan DelayTime()
        {
            var DailyTime = "16:00:00";
            var timeParts = DailyTime.Split(new char[1] { ':' });
            var dateNow = DateTime.Now;
            var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
            int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
            TimeSpan ts;
            if (date == dateNow)
                ts = date - dateNow;
            else
            {
                date.AddDays(1);
                ts = date - dateNow;
            }
            return ts;
        }
    }
}
