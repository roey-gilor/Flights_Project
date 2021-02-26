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
            return _flightDAO.GetAll();
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            return _ticketDAO.GetAll();
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
