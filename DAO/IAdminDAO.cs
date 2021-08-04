using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface IAdminDAO : IBasicDB<Administrator>
    {
        Administrator GetAdminByUserId(long id);
        IList<AirlineCompany> GetAllWaitingAirlines();
        void RemoveWaitingAirline(AirlineCompany airlineCompany);
        long AddWaitingAdmin(Administrator administrator);
        IList<Administrator> GetAllWaitingAdmins();
        void RemoveWaitingAdmin(Administrator administrator);
    }
}
