using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public interface ILoggedInAdministratorFacade
    {
        IList<Customer> GetAllCustomers(LoginToken<Administrator> token);
        IList<AirlineCompany> GetAllAirlines(LoginToken<Administrator> token);
        IList<Administrator> GetAllAdmins(LoginToken<Administrator> token);
        long CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline);
        void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline);
        void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline);
        long CreateNewCustomer(LoginToken<Administrator> token, Customer customer);
        void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer);
        void RemoveCustomer(LoginToken<Administrator> token, Customer customer);
        long CreateAdmin(LoginToken<Administrator> token, Administrator admin);
         void ModifyMyAdminUser(LoginToken<Administrator> token, Administrator admin);
        void UpdateAdmin(LoginToken<Administrator> token, Administrator admin);
        void RemoveAdmin(LoginToken<Administrator> token, Administrator admin);
        void CreateCountry(LoginToken<Administrator> token, Country country);
        void UpdateCountry(LoginToken<Administrator> token, Country country);
        void RemoveCountry(LoginToken<Administrator> token, Country country);
        void ChangeMyPassword(LoginToken<Administrator> token, string oldPassword, string newPassword);
        IList<AirlineCompany> GetWaitingAirlines(LoginToken<Administrator> token);
        long AcceptWaitingAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany);
        void RejectWaitingAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany);
        IList<Administrator> GetWaitingAdmins(LoginToken<Administrator> token);
        long AcceptWaitingAdmin(LoginToken<Administrator> token, Administrator admin);
        void RejectWaitingAdmin(LoginToken<Administrator> token, Administrator admin);
    }
}
