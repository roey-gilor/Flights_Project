using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoggedInCustomerFacade : AnonymousUserFacade, ILoggedInCustomerFacade
    {
        public void CancelTicket(LoginToken<Customer> token, Ticket ticket)
        {
            if (token != null)
            {
                _ticketDAO.Remove(ticket);
                ticket.Flight.Remaining_Tickets++;
                _flightDAO.Update(ticket.Flight);
            }
            else
                throw new WasntActivatedByCustomerException();
        }

        public void ChangeMyPassword(LoginToken<Customer> token, string oldPassword, string newPassword)
        {
            if (token != null)
            {
                if (token.User.User.Password != oldPassword)
                    throw new WrongCredentialsException();
                token.User.User.Password = newPassword;
                _userDAO.Update(token.User.User);
            }
            else
                throw new WasntActivatedByCustomerException();
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            if (token != null)
                return _flightDAO.GetFlightsByCustomer(token.User);
            else
                throw new WasntActivatedByCustomerException();
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
                            throw new CustomerAlreadyBoughtTicketException();
                    }
                }

                if (flight.Remaining_Tickets > 0)
                {
                    Ticket ticket = new Ticket { Flight_Id = flight.Id, Customer_Id = token.User.Id, Customer = token.User, Flight = flight };
                    _ticketDAO.Add(ticket);
                    flight.Remaining_Tickets--;
                    _flightDAO.Update(flight);
                    return ticket;
                }
                else
                    throw new OutOfTicketsExecption();
            }
            else
                throw new WasntActivatedByCustomerException();
        }
    }
}
