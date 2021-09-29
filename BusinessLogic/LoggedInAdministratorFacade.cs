using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    public class LoggedInAdministratorFacade : AnonymousUserFacade, ILoggedInAdministratorFacade
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void ChangeMyPassword(LoginToken<Administrator> token, string oldPassword, string newPassword)
        {
            if (token != null)
            {
                Administrator administrator = _adminDAO.Get(token.User.Id);
                User user = administrator.User;
                oldPassword = user.Password;
                if (user.Password != oldPassword)
                {
                    log.Error($"Discrepancies between {token.User.Id} {administrator.First_Name} {administrator.Last_Name} old password to the password that saved in the system");
                    throw new WrongCredentialsException($"Discrepancies between {token.User.Id} {administrator.First_Name} {administrator.Last_Name} old password to the password that saved in the system");
                }
                if (user.Password == newPassword)
                {
                    log.Error($"User {token.User.Id} tried to make his new password like the old one");
                    throw new WrongCredentialsException("New password can't be like the old one");
                }
                user.Password = newPassword;
                _userDAO.Update(user);
                log.Info($"Admin {token.User.Id} {administrator.First_Name} {administrator.Last_Name} changed password");
            }
            else
            {
                log.Error("An unknown user tried to change password");
                throw new WasntActivatedByAdministratorException("An unknown user tried to change password");
            }
        }

        public long CreateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level > admin.Level && administrator.Level == 3)
                {
                    try
                    {
                        long userId = _userDAO.Add(admin.User);
                        admin.User.Id = userId;
                        admin.User_Id = userId;
                        long id = _adminDAO.Add(admin);
                        log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} added new administrator: {admin.First_Name} {admin.Last_Name}");
                        return id;
                    }
                    catch (Exception ex)
                    {
                        if (admin.User_Id != 0)
                            _userDAO.Remove(admin.User);
                        log.Error($"faild to add user: {ex.Message}");
                        throw new DuplicateDetailsException($"faild to add user: {ex.Message}");
                    }
                }
                else
                {
                    if (administrator.Level == 3)
                    {
                        if (administrator.First_Name == "Main" && administrator.Last_Name == "admin")
                        {
                            try
                            {
                                long userId = _userDAO.Add(admin.User);
                                admin.User.Id = userId;
                                admin.User_Id = userId;
                                long id = _adminDAO.Add(admin);
                                log.Info($"Main admin added new administrator: {admin.First_Name} {admin.Last_Name}");
                                return id;
                            }
                            catch (Exception ex)
                            {
                                if (admin.User_Id != 0)
                                    _userDAO.Remove(admin.User);
                                log.Error($"faild to add user: {ex.Message}");
                                throw new DuplicateDetailsException($"faild to add user: {ex.Message}");
                            }
                        }
                        else
                        {
                            log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to add new administrator");
                            throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to add new administrator");
                        }
                    }
                    else
                    {
                        log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to add new administrator");
                        throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to add new administrator");
                    }
                }
            }
            else
            {
                log.Error("An unknown user tried to add administrator");
                throw new WasntActivatedByAdministratorException("An unknown user tried to add administrator");
            }
        }

        public void CreateCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level == 3)
                {
                    try
                    {
                        _countryDAO.Add(country);
                        log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} added new country: {country.Name}");
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Could not Add new country: {ex.Message}");
                        throw new DuplicateDetailsException($"Could not Add new country: {ex.Message}");
                    }
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to create new country in the Db");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction tocreate new country in the Db");
                }
            }
            else
            {
                log.Error("An unknown user tried to create new country");
                throw new WasntActivatedByAdministratorException("An unknown user tried to create new country");
            }
        }

        public long CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    try
                    {
                        long userId = _userDAO.Add(airline.User);
                        airline.User.Id = userId;
                        airline.User_Id = userId;
                        long id = _airlineDAO.Add(airline);
                        log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} added new airline: {airline.Name}");
                        return id;
                    }
                    catch (Exception ex)
                    {
                        if (airline.User_Id != 0)
                            _userDAO.Remove(airline.User);
                        log.Error($"Could not Add New airline company: {ex.Message}");
                        throw new DuplicateDetailsException($"Could not Add New airline company: {ex.Message}");
                    }
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to create new airline in the Db");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to create new airline in the Db");
                }
            }
            else
            {
                log.Error("An unknown user tried to create new airline");
                throw new WasntActivatedByAdministratorException("An unknown user tried to create new airline");
            }
        }

        public long CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    try
                    {
                        long userId = _userDAO.Add(customer.User);
                        customer.User_Id = userId;
                        customer.User.Id = userId;
                        long id = _customerDAO.Add(customer);
                        log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} added new customer: {customer.First_Name} {customer.Last_Name}");
                        return id;
                    }
                    catch (Exception ex)
                    {
                        if (customer.User_Id != 0)
                            _userDAO.Remove(customer.User);
                        log.Error($"Could not create new customer: {ex.Message}");
                        throw new DuplicateDetailsException($"Could not create new customer: {ex.Message}");
                    }
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to create new customer");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to create new customer");
                }
            }
            else
            {
                log.Error("An unknown user tried to create new customer");
                throw new WasntActivatedByAdministratorException("An unknown user tried to create new customer");
            }
        }

        public IList<Customer> GetAllCustomers(LoginToken<Administrator> token)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} got all customers from the system");
                return _customerDAO.GetAll();
            }
            else
            {
                log.Error("An unknown user tried to get all customers from the system");
                throw new WasntActivatedByAdministratorException("An unknown user tried to get all customers from the system");
            }
        }

        public void RemoveAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level > admin.Level && administrator.Level == 3)
                {
                    User user = admin.User;
                    _adminDAO.Remove(admin);
                    _userDAO.Remove(user);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} removed admin {admin.First_Name} {admin.Last_Name} from the system");
                }
                else
                {
                    if (administrator.Level == 3)
                    {
                        if (administrator.First_Name == "Main" && administrator.Last_Name == "admin")
                        {
                            User user = admin.User;
                            _adminDAO.Remove(admin);
                            _userDAO.Remove(user);
                            log.Info("Main admin removed admin from the system");
                        }
                        else
                        {
                            log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to add new administrator");
                            throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to add new administrator");
                        }
                    }
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to remove admins from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to remove admins from the system");
                }
            }
            else
            {
                log.Error("An unknown user tried to remove an admin from the system");
                throw new WasntActivatedByAdministratorException("An unknown user tried to remove an admin from the system");
            }
        }

        public void RemoveAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    IList<Flight> flights = _flightDAO.GetAll();
                    foreach (Flight flight in flights)
                    {
                        if (flight.Airline_Company_Id == airline.Id)
                        {
                            IList<Ticket> tickets = _ticketDAO.GetTicketsByFlight(flight);
                            foreach (Ticket ticket in tickets)
                            {
                                _ticketDAO.Remove(ticket);
                            }
                            _flightDAO.Remove(flight);
                        }
                    }
                    User user = airline.User;
                    _airlineDAO.Remove(airline);
                    _userDAO.Remove(user);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} removed airline {airline.Name} from the system");
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to remove airline {airline.Name} from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to remove airline {airline.Name} from the system");
                }
            }
            else
            {
                log.Error("An unknown user tried to remove an airline from the system");
                throw new WasntActivatedByAdministratorException("An unknown user tried to remove an airline from the system");
            }
        }

        public void RemoveCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level == 3)
                {
                    foreach (AirlineCompany airlineCompany in _airlineDAO.GetAll())
                    {
                        if (airlineCompany.Country_Id == country.Id)
                            RemoveAirline(token, airlineCompany);
                    }
                    IList<Flight> flights = _flightDAO.GetAll();
                    foreach (Flight flight in flights)
                    {
                        if (flight.Destination_Country_Id == country.Id || flight.Origin_Country_Id == country.Id)
                        {
                            IList<Ticket> tickets = _ticketDAO.GetTicketsByFlight(flight);
                            foreach (Ticket ticket in tickets)
                            {
                                _ticketDAO.Remove(ticket);
                            }
                            _flightDAO.Remove(flight);
                        }
                    }
                    _countryDAO.Remove(country);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} removed country {country.Name} from the system");

                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to remove country {country.Name} from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to remove country {country.Name} from the system");
                }
            }
            else
            {
                log.Error("An unknown user tried to remove a country from the system");
                throw new WasntActivatedByAdministratorException("An unknown user tried to remove a country from the system");
            }
        }

        public void RemoveCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    foreach (Ticket ticket in _ticketDAO.GetAll())
                    {
                        if (ticket.Customer_Id == customer.Id)
                        {
                            Flight flight = ticket.Flight;
                            flight.Remaining_Tickets++;
                            _flightDAO.Update(flight);
                            _ticketDAO.Remove(ticket);
                        }
                    }
                    User user = customer.User;
                    _customerDAO.Remove(customer);
                    _userDAO.Remove(user);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} removed customer {customer.First_Name} {customer.Last_Name} from the system");
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} didn't have sanction to remove customer {customer.First_Name} {customer.Last_Name} from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} didn't have sanction to remove customer {customer.First_Name} {customer.Last_Name} from the system");
                }
            }
            else
            {
                log.Error("An unknown user tried to remove a customer from the system");
                throw new WasntActivatedByAdministratorException("An unknown user tried to remove a customer from the system");
            }
        }
        public void ModifyMyAdminUser(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                User user = administrator.User;
                try
                {
                    UpdateUserDetails(token, admin.User);
                    _adminDAO.Update(admin);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} Updated it's details");
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change user {user.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change user {user.Id} details: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to update an admin details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update an admin details");
            }
        }
        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level > admin.Level && administrator.Level == 3)
                {
                    try
                    {
                        UpdateUserDetails(token, admin.User);
                        _adminDAO.Update(admin);
                        log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} Updated admin {admin.First_Name} {admin.Last_Name} details");
                    }
                    catch (Exception ex)
                    {
                        log.Error($"Could not change user {admin.User.Id} details: {ex.Message}");
                        throw new WrongCredentialsException($"Could not change user {admin.User.Id} details: {ex.Message}");
                    }
                }
                else
                {
                    if (administrator.Level == 3)
                    {
                        if (administrator.First_Name == "Main" && administrator.Last_Name == "admin")
                        {
                            try
                            {
                                UpdateUserDetails(token, admin.User);
                                _adminDAO.Update(admin);
                                log.Info($"Main Admin Updated admin {admin.First_Name} {admin.Last_Name} details");
                            }
                            catch (Exception ex)
                            {
                                log.Error($"Could not change user {admin.User.Id} details: {ex.Message}");
                                throw new WrongCredentialsException($"Could not change user {admin.User.Id} details: {ex.Message}");
                            }
                        }
                        else
                        {
                            log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to update admin {admin.First_Name} {admin.Last_Name} details");
                            throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to update admin {admin.First_Name} {admin.Last_Name} details");
                        }
                    }
                    else
                    {
                        log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to update admin {admin.First_Name} {admin.Last_Name} details");
                        throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to update admin {admin.First_Name} {admin.Last_Name} details");
                    }
                }
            }
            else
            {
                log.Error("An unknown user tried to update an admin details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update an admin details");
            }
        }

        public void UpdateAirlineDetails(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                try
                {
                    UpdateUserDetails(token, airline.User);
                    _airlineDAO.Update(airline);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} Updated airline {airline.Name} details");
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change user {airline.User.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change user {airline.User.Id} details: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to update an airline details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update an airline details");
            }
        }

        public void UpdateCountry(LoginToken<Administrator> token, Country country)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level == 3)
                {
                    try
                    {
                        _countryDAO.Update(country);
                        log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} Updated country {country.Name} details");

                    }
                    catch(Exception ex)
                    {
                        log.Error($"Could not change country {country.Name} details: {ex.Message}");
                        throw new WrongCredentialsException($"Could not change country {country.Name} details: {ex.Message}");
                    }
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to update country {country.Name} details");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to update country {country.Name} details");
                }
            }
            else
            {
                log.Error("An unknown user tried to update a country details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update a country details");
            }
        }

        public void UpdateCustomerDetails(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                try
                {
                    UpdateUserDetails(token, customer.User);
                    _customerDAO.Update(customer);
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} Updated customer {customer.First_Name} {customer.Last_Name} details");
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change user {administrator.User.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change user {administrator.User.Id} details: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to update a customer details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update a customer details");
            }
        }

        private void UpdateUserDetails(LoginToken<Administrator> token, User user)
        {
            if (token != null)
            {
                try
                {
                    _userDAO.Update(user);
                    log.Info($"User {user.Id} updated his details");
                }
                catch (Exception ex)
                {
                    log.Error($"Could not change user {user.Id} details: {ex.Message}");
                    throw new WrongCredentialsException($"Could not change user {user.Id} details: {ex.Message}");
                }
            }
            else
            {
                log.Error("An unknown user tried to update details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update details");
            }
        }

        public IList<AirlineCompany> GetWaitingAirlines(LoginToken<Administrator> token)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} got all waiting airlines");
                    return _adminDAO.GetAllWaitingAirlines();
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to get all waiting airlines");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to get all waiting airlines");
                }
            }
            else
            {
                log.Error("An unknown user tried to get all waiting airlines");
                throw new WasntActivatedByAdministratorException("An unknown user tried to get all waiting airlines");
            }
        }

        public long AcceptWaitingAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} approved waiting airlines");
                    _adminDAO.RemoveWaitingAirline(airlineCompany);
                    airlineCompany.User.User_Role = 2;
                   long id = CreateNewAirline(token, airlineCompany);
                    return id;
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to approve airline");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to approve airline");
                }
            }
            else
            {
                log.Error("An unknown user tried to approve airline");
                throw new WasntActivatedByAdministratorException("An unknown user tried to approve airline");
            }
        }

        public void RejectWaitingAirline(LoginToken<Administrator> token, AirlineCompany airlineCompany)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level != 1)
                {
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} rejected waiting airlines");
                    _adminDAO.RemoveWaitingAirline(airlineCompany);
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to reject airline");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to reject airline");
                }
            }
            else
            {
                log.Error("An unknown user tried to reject airline");
                throw new WasntActivatedByAdministratorException("An unknown user tried to reject airline");
            }
        }

        public IList<Administrator> GetWaitingAdmins(LoginToken<Administrator> token)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level == 3)
                {
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} got all waiting admins");
                    return _adminDAO.GetAllWaitingAdmins();
                }
                else
                {
                    log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to get all waiting admins");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to get all waiting admins");
                }
            }
            else
            {
                log.Error("An unknown user tried to get all waiting admins");
                throw new WasntActivatedByAdministratorException("An unknown user tried to get all waiting admins");
            }
        }

        public long AcceptWaitingAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level > admin.Level && administrator.Level == 3)
                {
                    log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} approved waiting admin");
                    _adminDAO.RemoveWaitingAdmin(admin);
                    admin.User.User_Role = 1;
                    long id = CreateAdmin(token, admin);
                    return id;
                }
                else
                {
                    if (administrator.Level == 3)
                    {
                        if (administrator.First_Name == "Main" && administrator.Last_Name == "admin")
                        {
                            log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} approved waiting admin");
                            _adminDAO.RemoveWaitingAdmin(admin);
                            admin.User.User_Role = 1;
                            long id = CreateAdmin(token, admin);
                            return id;
                        }
                        else
                        {
                            log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to approve waiting admin");
                            throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to approve waiting admin");
                        }
                    }
                    else
                    {
                        log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to approve waiting admin");
                        throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to approve waiting admin");
                    }
                }
            }
            else
            {
                log.Error("An unknown user tried to approve waiting admin");
                throw new WasntActivatedByAdministratorException("An unknown user tried to approve waiting admin");
            }
        }

        public void RejectWaitingAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                Administrator administrator = token.User.Id != 0 ? _adminDAO.Get(token.User.Id) : LoginService.mainAdmin;
                if (administrator.Level > admin.Level && administrator.Level == 3)
                {
                    _adminDAO.RemoveWaitingAdmin(admin);
                }
                else
                {
                    if (administrator.Level == 3)
                    {
                        if (administrator.First_Name == "Main" && administrator.Last_Name == "admin")
                        {
                            log.Info($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} rejected waiting admin");
                            _adminDAO.RemoveWaitingAdmin(admin);
                        }
                        else
                        {
                            throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to reject admin");
                        }
                    }
                    else
                    {
                        log.Error($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to reject admin");
                        throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {administrator.First_Name} {administrator.Last_Name} did not have sanction to reject admin");
                    }
                }
            }
            else
            {
                log.Error("An unknown user tried to reject admin");
                throw new WasntActivatedByAdministratorException("An unknown user tried to reject admin");
            }
        } 
    }
}
