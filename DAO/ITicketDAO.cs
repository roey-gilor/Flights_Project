using System;
using System.Collections.Generic;
using System.Text;

namespace DAO
{
    public interface ITicketDAO : IBasicDB<Ticket>
    {
        void Add_To_Tickets_History(Ticket ticket);
        IList<Ticket> GetTicketsByFlight(Flight flight);
    }
}
