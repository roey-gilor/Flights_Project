using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface IAirlineDAO : IBasicDB<AirlineCompany>
    {
        AirlineCompany GetAirlineByUserame(string name);
        IList<AirlineCompany> GetAllAirlinesByCountry(int countryId);
        AirlineCompany GetAirlineByUserId(long id);
        long AddWaitingAirline(AirlineCompany airlineCompany);
    }
}
