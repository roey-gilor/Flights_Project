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
            throw new NotImplementedException();
        }

        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            throw new NotImplementedException();
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            throw new NotImplementedException();
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            return _customerDAO.GetAll();
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            throw new NotImplementedException();
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token.User.Level != 1)
                _airlineDAO.Remove(airline);
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token.User.Level != 1)
                _customerDAO.Remove(customer);
        }

        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            throw new NotImplementedException();
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany customer)
        {
            _airlineDAO.Update(customer);
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            _customerDAO.Update(customer);
        }
    }
}
