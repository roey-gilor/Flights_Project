using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationProject.DTO
{
    public static class CountryDTO
    { 
        public static long ReturnIdFromName(string name)
        {
            CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();
            IList<Country> countries = countryDAOPGSQL.GetAll();
            foreach (Country country in countries)
            {
                if (country.Name == name)
                    return country.Id;
            }
            return 0;
        }
    }
}
