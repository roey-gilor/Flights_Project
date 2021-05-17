using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface IAdminDAO : IBasicDB<Administrator>
    {
        Administrator GetAdminByUserId(long id);
    }
}
