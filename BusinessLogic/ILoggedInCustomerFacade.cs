using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    public interface ILoggedInCustomerFacade
    {
        IList<Flight> GetAllMyFlights(LoginToken<Customer> token);
        Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight);
        void CancelTicket(LoginToken<Customer> token, Ticket ticket);
        void ChangeMyPassword(LoginToken<Customer> token, string oldPassword, string newPassword);
        void UpdateUserDetails(LoginToken<Customer> token, User user);
    }
}
