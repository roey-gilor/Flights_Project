using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void CreateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                    _adminDAO.Add(admin);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void CreateCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                if (token.User.Level == 3)
                    _countryDAO.Add(country);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _airlineDAO.Add(airline);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _customerDAO.Add(customer);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            if (token != null)
                return _customerDAO.GetAll();
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                    _adminDAO.Remove(admin);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _airlineDAO.Remove(airline);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void RemoveCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                if (token.User.Level == 3)
                    _countryDAO.Remove(country);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _customerDAO.Remove(customer);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                    _adminDAO.Update(admin);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
                _airlineDAO.Update(airline);
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void UpdateCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                if (token.User.Level == 3)
                    _countryDAO.Update(country);
                else
                    throw new AdministratorDoesntHaveSanctionException();
            }
            else
                throw new WasntActivatedByAdministratorException();
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
                _customerDAO.Update(customer);
            else
                throw new WasntActivatedByAdministratorException();
        }
    }
}
