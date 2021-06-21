﻿using DAO;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLogic
{
    public class LoggedInAirlineFacade : AnonymousUserFacade, ILoggedInAirlineFacade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void CancelFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token != null)
            {
                AirlineCompany airlineCompany = _airlineDAO.Get(token.User.Id);
                IList<Ticket> tickets = _ticketDAO.GetAll();
                foreach (Ticket ticket in tickets)
                {
                    if (flight.Id == ticket.Flight_Id)
                        _ticketDAO.Remove(ticket);
                }
                _flightDAO.Remove(flight);
                log.Info($"Airline {airlineCompany.Name} canceled flight {flight.Id}");
            }
            else
            {
                log.Error("An unknown user tried to cancel a flight");
                throw new WasntActivatedByAirlineException("An unknown user tried to cancel a flight");
            }
        }

        public void ChangeMyPassword(LoginToken<AirlineCompany> token, string oldPassword, string newPassword)
        {
            if (token != null)
            {
                if (token.User.User.Password != oldPassword)
                {
                    log.Error($"Discrepancies between {token.User.Name} old password to the password that saved in the system");
                    throw new WrongCredentialsException($"Discrepancies between {token.User.Name} old password to the password that saved in the system");
                }
                if (token.User.User.Password == newPassword)
                {
                    log.Error($"User {token.User.Id} tried to make his new password like the old one");
                    throw new WrongCredentialsException("New password can't be like the old one");
                }
                token.User.User.Password = newPassword;
                _userDAO.Update(token.User.User);
                log.Info($"Airline {token.User.Name} updated their password");
            }
            else
            {
                log.Error("An unknown user tried to update password");
                throw new WasntActivatedByAirlineException("An unknown user tried to update password");
            }
        }

        public Flight CreateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token != null)
            {
                try
                {
                    AirlineCompany airlineCompany = _airlineDAO.Get(token.User.Id);
                    flight.Airline_Company = airlineCompany;
                    flight.Airline_Company_Id = airlineCompany.Id;
                    long id = _flightDAO.Add(flight);
                    log.Info($"Airline {airlineCompany.Name} created flight {airlineCompany.Id}");
                    return _flightDAO.Get(id);
                }
                catch (Exception ex)
                {
                    log.Error($"Could not create new flight: {ex.Message}");
                    throw new Exception($"Could not create new flight: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to create a flight");
                throw new WasntActivatedByAirlineException("An unknown user tried to create a flight");
            }
        }

        public IList<Flight> GetAllFlights(LoginToken<AirlineCompany> token)
        {
            if (token != null)
            {
                AirlineCompany airlineCompany = _airlineDAO.Get(token.User.Id);
                List<Flight> flights = new List<Flight>();
                foreach (Flight flight in _flightDAO.GetAll())
                {
                    if (flight.Airline_Company == airlineCompany)
                    {
                        flights.Add(flight);
                    }
                }
                log.Info($"Airline {airlineCompany.Name} Got all it's flights");
                return flights;
            }
            else
            {
                log.Error("An unknown user tried to get all flights");
                throw new WasntActivatedByAirlineException("An unknown user tried to get all flights");
            }
        }

        public IList<Ticket> GetAllTickets(LoginToken<AirlineCompany> token)
        {
            if (token != null)
            {
                AirlineCompany airlineCompany = _airlineDAO.Get(token.User.Id);
                List<Ticket> tickets = new List<Ticket>();
                IList<Flight> flights = GetAllFlights(token);
                IList<Ticket> tickets_list = _ticketDAO.GetAll();
                foreach (Ticket ticket in tickets_list)
                {
                    if (flights.Contains(ticket.Flight))
                        tickets.Add(ticket);
                }
                log.Info($"Airline {airlineCompany.Name} Got all bought tickets from the system");
                return tickets;
            }
            else
            {
                log.Error("An unknown user tried to get all bought tickets of a airline company from the system");
                throw new WasntActivatedByAirlineException("An unknown user tried to get all bought tickets of a airline company from the system");
            }
        }

        public void MofidyAirlineDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            if (token != null)
            {
                try
                {
                    UpdateUserDetails(token, airline);
                    _airlineDAO.Update(airline);
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change Airkine {token.User.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change Airkine {token.User.Id} details: {ex.Message}");
                }
                log.Info($"Airline {token.User.Name} updated their details");
            }
            else
            {
                log.Error("An unknown user tried to update airline company details");
                throw new WasntActivatedByAirlineException("An unknown user tried to update airline company details");
            }
        }

        public void UpdateFlight(LoginToken<AirlineCompany> token, Flight flight)
        {
            if (token != null)
            {
                AirlineCompany airlineCompany = _airlineDAO.Get(token.User.Id);
                _flightDAO.Update(flight);
                log.Info($"Airline {airlineCompany.Name} updated flight {flight.Id} details");
            }
            else
            {
                log.Error("An unknown user tried to update airline company's flight details");
                throw new WasntActivatedByAirlineException("An unknown user tried to update airline company's flight details");
            }
        }

        private void UpdateUserDetails(LoginToken<AirlineCompany> token, AirlineCompany airline)
        {
            if (token != null)
            {
                try
                {
                    _userDAO.Update(airline.User);
                    log.Info($"User {token.User.User.Id} updated his details");
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change user {token.User.User.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change user {token.User.User.Id} details: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to update details");
                throw new WasntActivatedByAirlineException("An unknown user tried to update details");
            }
        }
    }
}
