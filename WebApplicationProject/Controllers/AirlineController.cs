using AutoMapper;
using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "Airline Company")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AirlineController : FlightControllerBase<AirlineCompany>
    {
        private ILoggedInAirlineFacade m_facade;
        private readonly IMapper m_mapper;
        public AirlineController(ILoggedInAirlineFacade airlineFacade, IMapper mapper)
        {
            m_facade = airlineFacade;
            m_mapper = mapper;
        }

        [HttpGet("GetAllAirlineFlights")]
        public async Task<ActionResult<IList<AirlineFlightDTO>>> GetAllAirlineFlights()
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            IList<Flight> flights = null;
            try
            {
                flights = await Task.Run(() => m_facade.GetAllFlights(token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{{ error: \"{ex.Message}\" }}");
            }
            if (flights.Count == 0)
            {
                return StatusCode(204, "{ }");
            }
            List<AirlineFlightDTO> flightDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO flightDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                flightDTOs.Add(flightDTO);
            }
            return Ok(JsonConvert.SerializeObject(flightDTOs));
        }

        [HttpGet("GetAllAirlineTickets")]
        public async Task<ActionResult<List<TicketDTO>>> GetAllAirlineTickets()
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            IList<Ticket> tickets;
            try
            {
                tickets = await Task.Run(() => m_facade.GetAllTickets(token));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{{ error: \"{ex.Message}\" }}");
            }
            if (tickets.Count == 0)
            {
                return StatusCode(204, "{ }");
            }
            List<TicketDTO> ticketDTOs = new List<TicketDTO>();
            foreach (Ticket ticket in tickets)
            {
                TicketDTO ticketDTO = m_mapper.Map<TicketDTO>(ticket);
                ticketDTOs.Add(ticketDTO);
            }
            return Ok(JsonConvert.SerializeObject(ticketDTOs));
        }

        [HttpPost("CreateNewFlight")]
        public async Task<ActionResult<AirlineFlightDTO>> CreateNewFlight([FromBody] AirlineFlightDTO airlineFlightDTO)
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            Flight flight = m_mapper.Map<Flight>(airlineFlightDTO);
            long id;
            try
            {
                id = await Task.Run(() => m_facade.CreateFlight(token, flight));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{{ error: \"{ex.Message}\" }}");
            }
            airlineFlightDTO.Id = id;
            AirlineFlightDTO flightDTO = m_mapper.Map<AirlineFlightDTO>(airlineFlightDTO);
            return Created($"api/Airline/CreateNewFlight/{flightDTO.Id}", JsonConvert.SerializeObject(flightDTO));
        }

        [HttpPut("UpdateFlight")]
        public async Task<ActionResult> UpdateFlight([FromBody] AirlineFlightDTO airlineFlightDTO)
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            Flight flight = m_mapper.Map<Flight>(airlineFlightDTO);
            try
            {
                await Task.Run(() => m_facade.UpdateFlight(token, flight));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }

        [HttpDelete("CancelFlight")]
        public async Task<ActionResult> CancelFlight([FromBody] AirlineFlightDTO airlineFlightDTO)
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            Flight flight = m_mapper.Map<Flight>(airlineFlightDTO);
            try
            {
                await Task.Run(() => m_facade.CancelFlight(token, flight));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{{ error: \"{ex.Message}\" }}");
            }
            return Ok();
        }
        [HttpPut("ChangeAirlinePassword")]
        public async Task<ActionResult> ChangeMyPassword([FromBody] UserDetailsDTO userDetails)
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
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
        [HttpPut("UpdateAirlineDetails")]
        public async Task<ActionResult> UpdateAirlineDetails([FromBody] AirlineDTO airline)
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            AirlineCompany airlineCompany = m_mapper.Map<AirlineCompany>(airline);
            try
            {
                await Task.Run(() => m_facade.MofidyAirlineDetails(token, airlineCompany));
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
    }
}
