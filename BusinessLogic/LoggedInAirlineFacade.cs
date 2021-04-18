using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token != null)
                _flightDAO.Remove(flight);
            else
                throw new WasntActivatedByAirlineException();
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            if (token != null)
            {
                if (token.User.User.Password != oldPassword)
                    throw new WrongCredentialsException();
                token.User.User.Password = newPassword;
                _userDAO.Update(token.User.User);
            }
            else
                throw new WasntActivatedByAirlineException();
        }

        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token != null)
            {
                flight.Airline_Company = token.User;
                flight.Airline_Company_Id = token.User.Id;
                _flightDAO.Add(flight);
            }
            else
                throw new WasntActivatedByAirlineException();
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            if (token != null)
            {
                List<Flight> flights = new List<Flight>();
                foreach (Flight flight in _flightDAO.GetAll())
                {
                    if (flight.Airline_Company == token.User)
                        flights.Add(flight);
                }
                return flights;
            }
            else
                throw new WasntActivatedByAirlineException();
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            if (token != null)
            {
                List<Ticket> tickets = new List<Ticket>();
                IList<Flight> flights = GetAllFlights(token);
                IList<Ticket> tickets_list = _ticketDAO.GetAll();
                foreach (Ticket ticket in tickets_list)
                {
                    if (flights.Contains(ticket.Flight))
                        tickets.Add(ticket);
                }
                return tickets;
            }
            else
                throw new WasntActivatedByAirlineException();
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            if (token != null)
                _airlineDAO.Update(airline);
            else
                throw new WasntActivatedByAirlineException();
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token != null)
                _flightDAO.Update(flight);
            else
                throw new WasntActivatedByAirlineException();
        }
    }
}
