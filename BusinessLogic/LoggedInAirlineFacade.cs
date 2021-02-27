using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            _flightDAO.Remove(flight);
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            token.User.User.Password = newPassword;
            _userDAO.Update(token.User.User);
        }

        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            flight.Airline_Company = token.User;
            flight.Airline_Company_Id = token.User.Id;
            _flightDAO.Add(flight);
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            List<Flight> flights = new List<Flight>();
            foreach (Flight flight in _flightDAO.GetAll())
            {
                if (flight.Airline_Company == token.User)
                    flights.Add(flight);
            }
            return flights;
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
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

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            _airlineDAO.Update(airline);
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            _flightDAO.Update(flight);
        }
    }
}
