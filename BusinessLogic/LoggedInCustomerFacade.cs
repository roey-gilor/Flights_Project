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
            _ticketDAO.Remove(ticket);
            ticket.Flight.Remaining_Tickets++;
            _flightDAO.Update(ticket.Flight);
        }

        public IList<Flight> GetAllMyFlights(LoginToken<Customer> token)
        {
            return _flightDAO.GetFlightsByCustomer(token.User);
        }

        public Ticket PurchaseTicket(LoginToken<Customer> token, Flight flight)
        {
            foreach (Ticket ticket in _ticketDAO.GetAll())
            {
                if (ticket.Flight_Id == flight.Id)
                {
                    if (ticket.Customer_Id == token.User.Id)
                        throw new CustomerAlreadyBoughtTicket();
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
    }
}
