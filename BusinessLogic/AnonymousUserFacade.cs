using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    public class AnonymousUserFacade : FacadeBase, IAnonymousUserFacade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public long AddNewCustomer(Customer customer)
        {
            try
            {
                long id = _userDAO.Add(customer.User);
                customer.User_Id = id;
                customer.User.Id = id;
                long customerId = _customerDAO.Add(customer);
                log.Info($"User {customer.User.Id} {customer.User.User_Name} was added to the system");
                return customerId;
            }
            catch (Exception ex)
            {
                if (customer.User_Id != 0)
                    _userDAO.Remove(customer.User);
                log.Error($"faild to add user: {ex.Message}");
                throw new DuplicateDetailsException($"faild to add user: {ex.Message}");
            }
        }

        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }

        public IList<Flight> GetAllFlights()
        {
            return _flightDAO.GetAll();
        }

        public Dictionary<Flight, int> GetAllFlightsVacancy()
        {
            return _flightDAO.GetAllFlightsVacancy();
        }

        public IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate)
        {
            return _flightDAO.GetFlightsByDepatrureDate(departureDate);
        }

        public IList<Flight> GetFlightsByDestinationCountry(int countryCode)
        {
            return _flightDAO.GetFlightsByDestinationCountry(countryCode);
        }

        public IList<Flight> GetFlightsByLandingDate(DateTime landingDate)
        {
            return _flightDAO.GetFlightsByLandingDate(landingDate);
        }

        public IList<Flight> GetFlightsByOriginCountry(int countryCode)
        {
            return _flightDAO.GetFlightsByOriginCountry(countryCode);
        }

    }
}
