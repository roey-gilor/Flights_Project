using AutoMapper;
using BusinessLogic;
using DAO;
using Microsoft.AspNetCore.Http;
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
    public class AnonymousController : ControllerBase
    {
        private IAnonymousUserFacade m_facade;
        private readonly IMapper m_mapper;
        public AnonymousController(IAnonymousUserFacade anonymousFacade, IMapper mapper)
        {
            m_facade = anonymousFacade;
            m_mapper = mapper;
        }
        [HttpPost("CreateNewCustomer")]
        public async Task<ActionResult<Customer>> CreateNewCustomer([FromBody] Customer customer)
        {
            long id;
            try
            {
                id = await Task.Run(() => m_facade.AddNewCustomer(customer));
            }
            catch (Exception ex)
            {
                return StatusCode(400, $"{{ error: \"{ex.Message}\" }}");
            }
            customer.Id = id;
            return Created($"api/Anonymous/CreateNewCustomer/{id}", JsonConvert.SerializeObject(customer));
        }
        [HttpGet("GetAllAirlineCompanies")]
        public async Task<ActionResult<List<AirlineReducedDTO>>> GetAllAirlineCompanies()
        {
            IList<AirlineCompany> airlines = await Task.Run(() => m_facade.GetAllAirlineCompanies());
            List<AirlineReducedDTO> airlineDTOs = new List<AirlineReducedDTO>();
            foreach (AirlineCompany airline in airlines)
            {
                AirlineReducedDTO airlineDTO = m_mapper.Map<AirlineReducedDTO>(airline);
                airlineDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineDTOs));
        }
        [HttpGet("GetAllFlights")]
        public async Task<ActionResult<List<AirlineReducedDTO>>> GetAllFlights()
        {
            IList<Flight> flights = await Task.Run(() => m_facade.GetAllFlights());
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetAllFlightsVacancy")]
        public async Task<ActionResult<Dictionary<CustomerFlightDTO, int>>> GetAllFlightsVacancy()
        {
            Dictionary<Flight, int> flightsVacancy = await Task.Run(() => m_facade.GetAllFlightsVacancy());
            Dictionary<CustomerFlightDTO, int> customerFlightsDTOs = new Dictionary<CustomerFlightDTO, int>();
            foreach (KeyValuePair<Flight, int> flight in flightsVacancy)
            {
                CustomerFlightDTO flightDTO = m_mapper.Map<CustomerFlightDTO>(flight.Key);
                customerFlightsDTOs.Add(flightDTO, flight.Value);
            }
            return Ok(JsonConvert.SerializeObject(customerFlightsDTOs));
        }
        [HttpGet("GetFlightsByDepatrureDate")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByDepatrureDate([FromBody] DateObjectDTO dateObject)
        {
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByDepatrureDate(dateObject.Date));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetFlightsByLandingDate")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByLandingDate([FromBody] DateObjectDTO dateObject)
        {
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByLandingDate(dateObject.Date));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetFlightsByDestinationCountry")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByDestinationCountry([FromBody] CountryDTO countryDTO)
        {
            long id = countryDTO.ReturnIdFromName(countryDTO.Country_Name);
            if (id == 0)
                return StatusCode(400, $"{{ error: \"{"Country name does not exists"}\" }}");
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByDestinationCountry((int)id));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
        [HttpGet("GetFlightsByOriginCountry")]
        public async Task<ActionResult<List<AirlineFlightDTO>>> GetFlightsByOriginCountry([FromBody] CountryDTO countryDTO)
        {
            long id = countryDTO.ReturnIdFromName(countryDTO.Country_Name);
            if (id == 0)
                return StatusCode(400, $"{{ error: \"{"Country name does not exists"}\" }}");
            IList<Flight> flights = await Task.Run(() => m_facade.GetFlightsByOriginCountry((int)id));
            List<AirlineFlightDTO> airlineFlightsDTOs = new List<AirlineFlightDTO>();
            foreach (Flight flight in flights)
            {
                AirlineFlightDTO airlineDTO = m_mapper.Map<AirlineFlightDTO>(flight);
                airlineFlightsDTOs.Add(airlineDTO);
            }
            return Ok(JsonConvert.SerializeObject(airlineFlightsDTOs));
        }
    }
}
