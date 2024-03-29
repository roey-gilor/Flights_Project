﻿using System;
using System.Collections.Generic;
using System.Text;
using DAO;

namespace BusinessLogic
{
    public interface IAnonymousUserFacade
    {
        IList<Flight> GetAllFlights();
        IList<AirlineCompany> GetAllAirlineCompanies();
        Dictionary<Flight, int> GetAllFlightsVacancy();
        IList<Flight> GetFlightsByOriginCountry(int countryCode);
        IList<Flight> GetFlightsByDestinationCountry(int countryCode);
        IList<Flight> GetFlightsByDepatrureDate(DateTime departureDate);
        IList<Flight> GetFlightsByLandingDate(DateTime landingDate);
        long AddNewCustomer(Customer customer);
        long AddNewWaitingAirline(AirlineCompany airlineCompany);
        IList<Country> GetAllCountries();
        long AddNewWaitingAdmin(Administrator administrator);

    }
}
