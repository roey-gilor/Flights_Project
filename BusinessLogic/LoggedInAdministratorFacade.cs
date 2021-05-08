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
                {
                    _adminDAO.Add(admin);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} added new administrator: {admin.First_Name} {admin.Last_Name}");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to add new administrator");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to add new administrator");
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
                if (token.User.Level == 3)
                {
                    _countryDAO.Add(country);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} added new country: {country.Name}");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to create new country in the Db");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction tocreate new country in the Db");
                }
            }
            else
            {
                log.Error("An unknown user tried to create new country");
                throw new WasntActivatedByAdministratorException("An unknown user tried to create new country");
            }
        }

        public void CreateNewAirline(LoginToken<Administrator> token, AirlineCompany airline)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                {
                    _airlineDAO.Add(airline);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} added new airline: {airline.Name}");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to create new airline in the Db");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction tocreate new airline in the Db");
                }
            }
            else
            {
                log.Error("An unknown user tried to create new airline");
                throw new WasntActivatedByAdministratorException("An unknown user tried to create new airline");
            }
        }

        public void CreateNewCustomer(LoginToken<Administrator> token, Customer customer)
        {
            if (token != null)
            {
                if (token.User.Level != 1)
                {
                    _customerDAO.Add(customer);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} added new customer: {customer.First_Name} {customer.Last_Name}");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to create new customer");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction tocreate new customer");
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
                log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} got all customers from the system");
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
                if (token.User.Level > admin.Level && token.User.Level == 3)
                {
                    _adminDAO.Remove(admin);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} removed admin {admin.First_Name} {admin.Last_Name} from the system");
                }
                else
                {
                    if (token.User.Level > admin.Level)
                    {
                        log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove admin {admin.First_Name} {admin.Last_Name} from the system");
                        throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove admin {admin.First_Name} {admin.Last_Name} from the system");
                    }
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove admins from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove admins from the system");
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
                if (token.User.Level != 1)
                {
                    _airlineDAO.Remove(airline);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} removed airline {airline.Name} from the system");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove airline {airline.Name} from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove airline {airline.Name} from the system");
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
                if (token.User.Level == 3)
                {
                    _countryDAO.Remove(country);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} removed country {country.Name} from the system");

                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove country {country.Name} from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to remove country {country.Name} from the system");
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
                if (token.User.Level != 1)
                {
                    _customerDAO.Remove(customer);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} removed customer {customer.First_Name} {customer.Last_Name} from the system");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} didn't have sanction to remove customer {customer.First_Name} {customer.Last_Name} from the system");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} didn't have sanction to remove customer {customer.First_Name} {customer.Last_Name} from the system");
                }
            }
            else
            {
                log.Error("An unknown user tried to remove a customer from the system");
                throw new WasntActivatedByAdministratorException("An unknown user tried to remove a customer from the system");
            }
        }

        public void UpdateAdmin(LoginToken<Administrator> token, Administrator admin)
        {
            if (token != null)
            {
                if (token.User.Level > admin.Level && token.User.Level == 3)
                {
                    _adminDAO.Update(admin);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} Updated admin {admin.First_Name} {admin.Last_Name} details");
                }
                else
                {
                    if (token.User.Level > admin.Level)
                    {
                        log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to update admin {admin.First_Name} {admin.Last_Name} details");
                        throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to update admin {admin.First_Name} {admin.Last_Name} details");
                    }
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to update admins details");
                    throw new AdministratorDoesntHaveSanctionException($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to update admins details");
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
                _airlineDAO.Update(airline);
                log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} Updated airline {airline.Name} details");
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
                if (token.User.Level == 3)
                {
                    _countryDAO.Update(country);
                    log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} Updated country {country.Name} details");
                }
                else
                {
                    log.Error($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} did not have sanction to update country {country.Name} details");
                    throw new AdministratorDoesntHaveSanctionException();
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
                _customerDAO.Update(customer);
                log.Info($"{token.User.Id} {token.User.First_Name} {token.User.Last_Name} Updated customer {customer.First_Name} {customer.Last_Name} details");
            }
            else
            {
                log.Error("An unknown user tried to update a customer details");
                throw new WasntActivatedByAdministratorException("An unknown user tried to update a customer details");
            }
        }
    }
}
