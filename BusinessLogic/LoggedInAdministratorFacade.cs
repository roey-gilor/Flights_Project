using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        public void CreateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                    _adminDAO.Add(admin);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void CreateCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                if (token.User.Level == 3)
                    _countryDAO.Add(country);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _airlineDAO.Add(airline);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _customerDAO.Add(customer);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            if (token != null)
                return _customerDAO.GetAll();
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                    _adminDAO.Remove(admin);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _airlineDAO.Remove(airline);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void RemoveCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                if (token.User.Level == 3)
                    _countryDAO.Remove(country);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                    _customerDAO.Remove(customer);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                    _adminDAO.Update(admin);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
                _airlineDAO.Update(airline);
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void UpdateCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                if (token.User.Level == 3)
                    _countryDAO.Update(country);
                else
                    throw new AdministratorDoesntHaveSanctionExecption();
            }
            else
                throw new WasntActivatedByAdminstratorExecption();
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
                _customerDAO.Update(customer);
            else
                throw new WasntActivatedByAdminstratorExecption();
        }
    }
}
