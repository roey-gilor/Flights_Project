using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (token != null)
            {
                Flight flight = _flightDAO.Get(ticket.Flight_Id);
                flight.Remaining_Tickets++;
                _flightDAO.Update(flight);
                _ticketDAO.Remove(ticket);
                log.Info($"Customer {token.User.Id} {token.User.First_Name} {token.User.Last_Name} canceled ticket purchasing");
            }
            else
            {
                log.Error("An unknown user tried to cancel ticket purchasing");
                throw new WasntActivatedByCustomerException("An unknown user tried to cancel ticket purchasing");
            }
        }

        public void ChangeMyPassword(LoginToken<Customer> token, string oldPassword, string newPassword)
        {
            if (token != null)
            {
                if (token.User.User.Password != oldPassword)
                {
                    log.Error($"Discrepancies between {token.User.Id} {token.User.First_Name} {token.User.Last_Name} old password to the password that saved in the system");
                    throw new WrongCredentialsException($"Discrepancies between {token.User.Id} {token.User.First_Name} {token.User.Last_Name} old password to the password that saved in the system");
                }
                if (token.User.User.Password == newPassword)
                {
                    log.Error($"User {token.User.Id} tried to make his new password like the old one");
                    throw new WrongCredentialsException("New password can't be like the old one");
                }
                token.User.User.Password = newPassword;
                _userDAO.Update(token.User.User);
                log.Info($"Customer {token.User.Id} {token.User.First_Name} {token.User.Last_Name} changed password");
            }
            else
            {
                log.Error("An unknown user tried to change password");
                throw new WasntActivatedByCustomerException("An unknown user tried to change password");
            }
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            if (token != null)
            {
                log.Info($"Customer {token.User.Id} Got his flights");
                return _flightDAO.GetFlightsByCustomer(token.User);
            }
            else
            {
                log.Error("An unknown user tried to Get all system flights");
                throw new WasntActivatedByCustomerException("An unknown user tried to Get all system flights");
            }
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            if (token != null)
            {
                foreach (Ticket ticket in _ticketDAO.GetAll())
                {
                    if (ticket.Flight_Id == flight.Id)
                    {
                        if (ticket.Customer_Id == token.User.Id)
                        {
                            log.Error($"Customer {token.User.Id} {token.User.First_Name} {token.User.Last_Name} tried to buy a ticket to the flight twice");
                            throw new CustomerAlreadyBoughtTicketException($"Customer {token.User.Id} {token.User.First_Name} {token.User.Last_Name} tried to buy a ticket to the flight twice");
                        }
                    }
                }

                if (flight.Remaining_Tickets > 0)
                {
                    Ticket ticket = new Ticket { Flight_Id = flight.Id, Customer_Id = token.User.Id, Customer = token.User, Flight = flight };
                    ticket.Id = _ticketDAO.Add(ticket);
                    flight.Remaining_Tickets--;
                    _flightDAO.Update(flight);
                    log.Info($"Customer {token.User.Id} {token.User.First_Name} {token.User.Last_Name} bought a ticket");
                    return ticket;
                }
                else
                {
                    log.Error($"There are not any left tickets for flight {flight.Id}");
                    throw new OutOfTicketsException($"There are not any left tickets for flight {flight.Id}");
                }
            }
            else
            {
                log.Error("An unknown user tried to buy a ticket");
                throw new WasntActivatedByCustomerException("An unknown user tried to buy a ticket");
            }
        }

        public void UpdateUserDetails(LoginToken<Customer> token, Customer customer)
        {
            if (token != null)
            {
                try
                {
                    _userDAO.Update(customer.User);
                    _customerDAO.Update(customer);
                    log.Info($"Customer {token.User.Id} updated his details");
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change customer {token.User.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change customer {token.User.Id} details: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to update details");
                throw new WasntActivatedByCustomerException("An unknown user tried to update details");
            }
        }
    }
}
