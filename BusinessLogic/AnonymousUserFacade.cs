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

        public long AddNewWaitingAdmin(Administrator administrator)
        {
            try
            {
                List<User> users = (List<User>)_userDAO.GetAll();
                if (users.Exists(user => user.User_Name == administrator.User.User_Name))
                {
                    log.Error("faild to add user: User name is already taken");
                    throw new DuplicateDetailsException("User name is already taken");
                }
                if (users.Exists(user => user.Email == administrator.User.Email))
                {
                    log.Error("faild to add user: Email is already taken");
                    throw new DuplicateDetailsException("Email is already taken");
                }
                return _adminDAO.AddWaitingAdmin(administrator);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("_"))
                    throw new DuplicateDetailsException(ex.Message);
                log.Error($"faild to add user: {ex.Message}");
                string error = ex.Message.Split("\"")[1].Split("_")[2];
                throw new DuplicateDetailsException($"{error} is already taken");
            }
        }

        public long AddNewWaitingAirline(AirlineCompany airlineCompany)
        {
            try
            {
                List<AirlineCompany> airlineCompanies = (List<AirlineCompany>)_airlineDAO.GetAll();
                List<User> users = (List<User>)_userDAO.GetAll();
                if (airlineCompanies.Exists(airline => airline.Name == airlineCompany.Name))
                {
                    log.Error("faild to add user: Company name is already taken");
                    throw new DuplicateDetailsException("Company name is already taken");
                }
                if (users.Exists(user => user.User_Name == airlineCompany.User.User_Name))
                {
                    log.Error("faild to add user: User name is already taken");
                    throw new DuplicateDetailsException("User name is already taken");
                }
                if (users.Exists(user => user.Email == airlineCompany.User.Email))
                {
                    log.Error("faild to add user: Email is already taken");
                    throw new DuplicateDetailsException("Email is already taken");
                }
                return _airlineDAO.AddWaitingAirline(airlineCompany);
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("_"))
                    throw new DuplicateDetailsException(ex.Message);
                log.Error($"faild to add user: {ex.Message}");
                string error = ex.Message.Split("\"")[1].Split("_")[3] == "uindex" ? ex.Message.Split("\"")[1].Split("_")[2] :
                   ex.Message.Split("\"")[1].Split("_")[2] + " " + ex.Message.Split("\"")[1].Split("_")[3];
                throw new DuplicateDetailsException($"{error} is already taken");
            }
        }

        public IList<AirlineCompany> GetAllAirlineCompanies()
        {
            return _airlineDAO.GetAll();
        }

        public IList<Country> GetAllCountries()
        {
            return _countryDAO.GetAll();
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
