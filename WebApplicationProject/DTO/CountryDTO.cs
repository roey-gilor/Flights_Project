using DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationProject.DTO
{
    public class CountryDTO
    {
        public string Country_Name { get; set; }
        CountryDAOPGSQL countryDAOPGSQL = new CountryDAOPGSQL();      
        public long ReturnIdFromName(string name)
        {
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
