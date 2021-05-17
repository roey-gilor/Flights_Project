using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface ICustomerDAO:IBasicDB<Customer>
    {
        Customer GetCustomerByUserame(string name);
        Customer GetCustomerByUserId(long id);
    }
}
