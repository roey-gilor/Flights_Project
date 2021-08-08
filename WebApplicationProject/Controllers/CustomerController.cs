using AutoMapper;
using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationProject.DTO;

namespace WebApplicationProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController : FlightControllerBase<Customer>
    {
        private ILoggedInCustomerFacade m_facade;
        private readonly IMapper m_mapper;
        public CustomerController(ILoggedInCustomerFacade customerFacade, IMapper mapper)
        {
            m_facade = customerFacade;
            m_mapper = mapper;
        }

        [HttpGet("GetAllCustomerFlights")]
        public async Task<ActionResult<IList<CustomerFlightDTO>>> GetAllCustomerFlights()
        {
            LoginToken<Customer> token = GetLoginToken();
            Dictionary<long, long> mapFlightsToTickets = new Dictionary<long, long>();
            IList<Flight> flights = null;
            try
            {
                Task<IList<Flight>> flightsTask = Task<IList<Flight>>.Factory.StartNew(() => m_facade.GetAllMyFlights(token));
                flightsTask.Wait();
                flights = flightsTask.Result;
                mapFlightsToTickets = await Task.Run(() => m_facade.GetAllTicketsIdByFlightsId(token, flights));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{{ error: \"{ex.Message}\" }}");
            }
            if (flights.Count == 0)
            {
                return StatusCode(204, "{ }");
            }
            List<CustomerFlightDTO> flightDTOs = new List<CustomerFlightDTO>();
            foreach (Flight flight in flights)
            {
                CustomerFlightDTO flightDTO = m_mapper.Map<CustomerFlightDTO>(flight);
                flightDTO.Ticket_Id = mapFlightsToTickets[flight.Id];
                flightDTOs.Add(flightDTO);
            }
            return Ok(JsonConvert.SerializeObject(flightDTOs));

        }

        [HttpPut("UpdateCustomerDetails")]
        public async Task<ActionResult> UpdateUserDetails([FromBody] Customer customer)
        {
            LoginToken<Customer> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.UpdateUserDetails(token, customer));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByCustomerException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }

        [HttpPost("PurchaseTicket")]
        public async Task<ActionResult<TicketDTO>> PurchaseTicket([FromBody] AirlineFlightDTO flightDTO)
        {
            LoginToken<Customer> token = GetLoginToken();
            Flight flight = m_mapper.Map<Flight>(flightDTO);
            Ticket ticket = null;
            try
            {
                ticket = await Task.Run(() => { return m_facade.PurchaseTicket(token, flight); });
            }
            catch (CustomerAlreadyBoughtTicketException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (OutOfTicketsException ex)
            {
                return StatusCode(406, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByCustomerException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            TicketDTO ticketDTO = m_mapper.Map<TicketDTO>(ticket);
            return Created($"api/Customer/PurchaseTicket/{ticketDTO.Id}", JsonConvert.SerializeObject(ticketDTO));
        }

        [HttpPut("ChangeCustomerPassword")]
        public async Task<ActionResult> ChangeMyPassword([FromBody] UserDetailsDTO userDetails)
        {
            LoginToken<Customer> token = GetLoginToken();
            try
            {
                await Task.Run(() => m_facade.ChangeMyPassword(token, token.User.Password, userDetails.Password));
            }
            catch (WrongCredentialsException ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            catch (WasntActivatedByCustomerException ex)
            {
                return StatusCode(401, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }

        [HttpDelete("CancelTicketPurchase")]
        public async Task<ActionResult> CancelTicketPurchase([FromBody] TicketDTO ticketDTO)
        {
            LoginToken<Customer> token = GetLoginToken();
            Ticket ticket = m_mapper.Map<Ticket>(ticketDTO);
            try
            {
                await Task.Run(() => m_facade.CancelTicket(token, ticket));
            }
            catch (Exception ex)
            {
                return StatusCode(403, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
    }
}
