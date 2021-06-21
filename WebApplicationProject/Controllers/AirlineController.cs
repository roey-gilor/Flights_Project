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
        public async Task<ActionResult<IList<Flight>>> GetAllAirlineFlights()
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

        // POST api/<AirlineController>
        [HttpPost("CreateNewFlight")]
        public async Task<ActionResult<AirlineFlightDTO>> Post([FromBody] AirlineFlightDTO airlineFlightDTO)
        {
            LoginToken<AirlineCompany> token = GetLoginToken();
            Flight flight = m_mapper.Map<Flight>(airlineFlightDTO);
            Flight createdFlight;
            try
            {
                createdFlight = await Task.Run(() => m_facade.CreateFlight(token, flight));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"{{ error: \"{ex.Message}\" }}");
            }
            AirlineFlightDTO flightDTO = m_mapper.Map<AirlineFlightDTO>(createdFlight);
            return Created($"api/Airline/CreateNewFlight/{flightDTO.Id}", JsonConvert.SerializeObject(flightDTO));
        }

        // PUT api/<AirlineController>/5
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

        // DELETE api/<AirlineController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
