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
                try
                {
                    if (m_Instance == null)
                    {
                        lock (key)
                        {
                            if (m_Instance == null)
                                m_Instance = new FlightCenterSystem();
                        }
                    }
                    log.Info("Someone has connected to the system");
                    return m_Instance;
                }
                catch (Exception ex)
                {
                    log.Fatal($"Could not connect in to the system: {ex.Message}");
                    throw new Exception($"Could not connect in to the system: {ex.Message}");
                }
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
                    try
                    {
                        if (DateTime.Now.TimeOfDay == AppConfig.Instance.WakingUpTime.TimeOfDay)
                        {
                            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                DateTime.Now.Hour - 3, DateTime.Now.Minute, DateTime.Now.Second);
                            IList<Flight> flights = flightDAOPGSQL.GetOldFlights(date);
                            foreach (Flight flight in flights)
                            {
                                IList<Ticket> tickets = ticketDAOPGSQL.GetTicketsByFlight(flight);
                                foreach (Ticket ticket in tickets)
                                {
                                    ticketDAOPGSQL.Add_To_Tickets_History(ticket);
                                    ticketDAOPGSQL.Remove(ticket);
                                }
                                flightDAOPGSQL.Add_to_flights_history(flight);
                                flightDAOPGSQL.Remove(flight);
                            }
                            log.Info($"Old flights and ticket were transformed to archive at {DateTime.Now.Date}");
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Could not transform old tickets and flights to the archive: {ex.Message}");
                    }
                }
            });
            thread.Start();

        }
        public bool Login(out ILoginToken loginToken, string username, string password)
        {
            try
            {
                bool res = loginService.TryLogin(out loginToken, username, password);
                return res;
            }
            catch (Exception ex) 
            {
                throw new Exception($"{ex.Message}");
            }
        }
        public FacadeBase GetFacade<T>(LoginToken<T> token) where T : IUser
        {
            if (typeof(T) == typeof(Administrator))
                return new LoggedInAdministratorFacade();
            else
            {
                if (typeof(T) == typeof(AirlineCompany))
                    return new LoggedInAirlineFacade();
                else
                    return new LoggedInCustomerFacade();
            }
        }
    }
}
