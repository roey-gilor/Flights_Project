using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    class CountryDAOPGSQL : ICountryDAO
    {
        void IBasicDB<Country>.Add(Country t)
        {
            throw new NotImplementedException();
        }

        Country IBasicDB<Country>.Get(int id)
        {
            throw new NotImplementedException();
        }

        IList<Country> IBasicDB<Country>.GetAll()
        {
            throw new NotImplementedException();
        }

        void IBasicDB<Country>.Remove(Country t)
        {
            throw new NotImplementedException();
        }

        void IBasicDB<Country>.Update(Country t)
        {
            throw new NotImplementedException();
        }
    }
}
