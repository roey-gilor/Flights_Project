using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAO;

namespace BusinessLogic
{
    public class FlightCenterSystem
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            Thread thread = new Thread(() =>
            {
                FlightDAOPGSQL flightDAOPGSQL = new FlightDAOPGSQL();
                TicketDAOPGSQL ticketDAOPGSQL = new TicketDAOPGSQL();
                while (true)
                {
                    if (DateTime.Now.TimeOfDay == AppConfig.Instance.WakingUpTime.TimeOfDay)
                    {
                        DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            DateTime.Now.Hour - 3, DateTime.Now.Minute, DateTime.Now.Second);
                        IList<Flight> flights = flightDAOPGSQL.GetOldFlights(date);
                        foreach (Flight flight in flights)
                        {
                            foreach (Ticket ticket in ticketDAOPGSQL.GetTicketsByFlight(flight))
                            {
                                ticketDAOPGSQL.Add_To_Tickets_History(ticket);
                                ticketDAOPGSQL.Remove(ticket);
                            }
                            flightDAOPGSQL.Add_to_flights_history(flight);
                            flightDAOPGSQL.Remove(flight);
                        }
                    }
                }
            });
            thread.Start();

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
                throw new WrongCredentialsException("One or more of the details are wrong");
            }
        }        
    }
}
