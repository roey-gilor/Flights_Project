using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            throw new NotImplementedException();
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            throw new NotImplementedException();
        }

        public IList<Ticket> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            throw new NotImplementedException();
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            throw new NotImplementedException();
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            throw new NotImplementedException();
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            throw new NotImplementedException();
        }
    }
}
