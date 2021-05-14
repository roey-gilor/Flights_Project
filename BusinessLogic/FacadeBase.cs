using System;
using DAO;

namespace BusinessLogic
{
    public class FacadeBase
    {
        protected IAirlineDAO _airlineDAO = new AirlineDAOPGSQL();
        protected ICountryDAO _countryDAO = new CountryDAOPGSQL();
        protected ICustomerDAO _customerDAO = new CustomerDAOPGSQL();
        protected IAdminDAO _adminDAO = new AdminDAOPGSQL();
        protected IUserDAO _userDAO = new UserDAOPGSQL();
        protected IFlightDAO _flightDAO = new FlightDAOPGSQL();
        protected ITicketDAO _ticketDAO = new TicketDAOPGSQL();

    }
}
